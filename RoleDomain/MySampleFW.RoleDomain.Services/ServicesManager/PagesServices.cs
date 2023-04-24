using System.Linq.Expressions;
using MySampleFW.RoleDomain.Services.CacheInterfaces;
using MySampleFW.Helper.Validations.Interfaces;
using MyCore.LogManager.ExceptionHandling;
using MySampleFW.Helper.Maps;
using MySampleFW.RoleDomain.Services.Interfaces;
using MyCore.Common.Base;
using MySampleFW.RoleDomain.Libraries.Models;
using MySampleFW.RoleDomain.Libraries.Entities;
using MyCore.Common.Helper;
using MySampleFW.RoleDomain.Repositores.Interfaces;

namespace MySampleFW.RoleDomain.Services.ServicesManager;

public class PagesServices : IPagesServices
{
    #region private
    private IPagesRepository pagesRepository;
    private IPagesCache cache;
    private IPageObjectCache pageObjectCache;
    private IValidateManager validate;
    #endregion

    #region Ctor
    public PagesServices(
        IPagesRepository _pagesRepository,
        IPagesCache _cache,
        IPageObjectCache _pageObjectCache,
        IValidateManager _validate)
    {
        pagesRepository = _pagesRepository;
        cache = _cache;
        pageObjectCache = _pageObjectCache;
        validate = _validate;
    }
    #endregion

    #region Methods
    public ResponseBase<PagesModel> CreateOrUpdate(RequestBase<PagesCreateOrUpdateModel> request)
    {
        var rData = request.RequestData;
        if (!DataValidation(rData.PagesName, rData.ID))
            return ResponseHelper.ErrorResponse<PagesModel>(ExceptionMessageHelper.IsInUse("Page"));

        var entity = MapperInstance.Instance.Map<PagesCreateOrUpdateModel, PagesEntity>(rData);

        var validateResult = validate.RolesValidate(entity).Result;
        if (validateResult.Any())
            return ResponseHelper.ErrorResponse<PagesModel>(validateResult);

        var result = (rData.ID.HasValue)
            ? pagesRepository.Update(entity, request.RequestUserId)
            : pagesRepository.Create(entity, request.RequestUserId);

        if (result.IsCompletedSuccessfully && !result.Id.IsNullOrLessOrEqToZero())
        {
            var returnModel = MapperInstance.Instance.Map<PagesEntity, PagesModel>(result.Result);
            cache.AddSingleData(returnModel);
            return ResponseHelper.SuccessResponse(returnModel);
        }
        return ResponseHelper.ErrorResponse<PagesModel>(ExceptionMessageHelper.ProcessFailedResult);
    }
    public ResponseBase<PagesModel> ChangeStatus(RequestBase<PagesStatusChangeModel> request)
    {
        var rData = request.RequestData;
        if (rData.ID.IsNullOrLessOrEqToZero())
            return ResponseHelper.ErrorResponse<PagesModel>(ExceptionMessageHelper.RequiredField("ID"), ResultEnum.Warning);
        var predicate = PredicateBuilderHelper.False<PagesModel>();
        predicate = predicate.And(q => q.ID == rData.ID);
        predicate = predicate.And(q => q.ActivationStatus == rData.ActivationStatus);

        var model = cache.GetSingleDataByFilter(predicate);
        model.ActivationStatus = rData.ActivationStatus;
        var entity = MapperInstance.Instance.Map<PagesModel, PagesEntity>(model);
        var result = pagesRepository.Update(entity, request.RequestUserId);
        if (result.IsNotNullOrEmpty())
        {
            cache.ReFillCache();
            return ResponseHelper.SuccessResponse(model);
        }
        return ResponseHelper.ErrorResponse<PagesModel>(ExceptionMessageHelper.ProcessFailedResult);
    }
    public ResponseBase<IQueryable<PagesModel>> GetDataByFilter(PagesFilterModel request)
    {
        var data = cache.GetAllData();
        var response = data.Where(GetPredicate(request));
        return (response.Any())
            ? ResponseHelper.SuccessResponse(response)
            : ResponseHelper.ErrorResponse<IQueryable<PagesModel>>(ExceptionMessageHelper.DataNotFound);
    }
    public ResponseBase<PagesModel> GetSingleDataByFilter(PagesFilterModel request)
    {
        var data = cache.GetAllData();
        var response = data.FirstOrDefault(GetPredicate(request));
        var pageObjects = pageObjectCache.GetDataByPageId(request.Id);
        if (pageObjects.Any())
            response.PagesObjects = pageObjects;
        return (response.IsNotNullOrEmpty())
            ? ResponseHelper.SuccessResponse(response)
            : ResponseHelper.ErrorResponse<PagesModel>(ExceptionMessageHelper.DataNotFound);
    }
    public Task<IQueryable<UserRolesPagesRepository>> GetPagesByUserID(int userID)
    {
        return pagesRepository.GetPagesByUserID(userID);
    }
    private Expression<Func<PagesModel, bool>> GetPredicate(PagesFilterModel request)
    {
        var predicate = PredicateBuilderHelper.False<PagesModel>();
        if (!request.ActivationStatus.HasValue)
            predicate = predicate.And(q => q.ActivationStatus == (int)ActivationStatusEnum.Active);
        else
            predicate = predicate.And(q => q.ActivationStatus == request.ActivationStatus);

        predicate = predicate.And(q => q.PageName.ToLower().Contains(request.PageName.ToLower()));
        return predicate;
    }
    private bool DataValidation(string pageName, int? id)
    {
        var data = cache.GetAllData();
        var predicate = PredicateBuilderHelper.False<PagesModel>();
        predicate = predicate.And(q => q.PageName.ToLower() == pageName.ToLower());
        predicate = predicate.And(q => q.ActivationStatus == (int)ActivationStatusEnum.Active);
        var result = data.FirstOrDefault(predicate);
        if (result != null)
        {
            if (id.HasValue && id == result.ID)
                return true;
            return false;
        }
        return true;
    }
    #endregion
}
