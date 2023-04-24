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
public class PageObjectServices : IPageObjectServices
{
    #region private
    private IPageObjectRepository PageObjectRepository;
    private IPageObjectCache cache;
    private IPagesCache pageCache;
    private IValidateManager validate;
    #endregion

    #region Ctor
    public PageObjectServices(
        IPageObjectRepository _PageObjectRepository,
        IPageObjectCache _cache,
        IPagesCache _pageCache,
        IValidateManager _validate)
    {
        PageObjectRepository = _PageObjectRepository;
        cache = _cache;
        pageCache = _pageCache;
        validate = _validate;
    }
    #endregion

    #region Methods
    public ResponseBase<PageObjectModel> CreateOrUpdate(RequestBase<PageObjectCreateOrUpdateModel> request)
    {
        var rData = request.RequestData;
        if (DataValidation(rData.PageObjectName, rData.ID))
            return ResponseHelper.ErrorResponse<PageObjectModel>(ExceptionMessageHelper.IsInUse("Page Object"));

        var entity = MapperInstance.Instance.Map<PageObjectCreateOrUpdateModel, PageObjectEntity>(rData);

        var validateResult = validate.RolesValidate(entity).Result;
        if (validateResult.Any())
            return ResponseHelper.ErrorResponse<PageObjectModel>(validateResult);

        var result = (rData.ID.HasValue)
            ? PageObjectRepository.Update(entity, request.RequestUserId)
            : PageObjectRepository.Create(entity, request.RequestUserId);

        if (result.IsCompletedSuccessfully && !result.Id.IsNullOrLessOrEqToZero())
        {
            var returnModel = MapperInstance.Instance.Map<PageObjectEntity, PageObjectModel>(result.Result);
            cache.AddSingleData(returnModel);
            return ResponseHelper.SuccessResponse(returnModel);
        }
        return ResponseHelper.ErrorResponse<PageObjectModel>(ExceptionMessageHelper.ProcessFailedResult);
    }
    public ResponseBase<PageObjectModel> ChangeStatus(RequestBase<PageObjectStatusChangeModel> request)
    {
        var rData = request.RequestData;
        if (rData.ID.IsNullOrLessOrEqToZero())
            return ResponseHelper.ErrorResponse<PageObjectModel>(ExceptionMessageHelper.RequiredField("ID"), ResultEnum.Warning);
        var predicate = PredicateBuilderHelper.False<PageObjectModel>();
        predicate = predicate.And(q => q.ID == rData.ID);
        predicate = predicate.And(q => q.ActivationStatus == rData.ActivationStatus);

        var model = cache.GetSingleDataByFilter(predicate);
        model.ActivationStatus = rData.ActivationStatus;
        var entity = MapperInstance.Instance.Map<PageObjectModel, PageObjectEntity>(model);
        var result = PageObjectRepository.Update(entity, request.RequestUserId);
        if (result.IsNotNullOrEmpty())
        {
            cache.ReFillCache();
            return ResponseHelper.SuccessResponse(model);
        }
        return ResponseHelper.ErrorResponse<PageObjectModel>(ExceptionMessageHelper.ProcessFailedResult);
    }
    public ResponseBase<IQueryable<PageObjectModel>> GetDataByFilter(PageObjectFilterModel request)
    {
        var data = cache.GetAllData();
        var response = data.Where(GetPredicate(request));
        return (response.Any())
            ? ResponseHelper.SuccessResponse(response)
            : ResponseHelper.ErrorResponse<IQueryable<PageObjectModel>>(ExceptionMessageHelper.DataNotFound);
    }
    public ResponseBase<PageObjectModel> GetSingleDataByFilter(PageObjectFilterModel request)
    {
        var data = cache.GetAllData();
        var response = data.FirstOrDefault(GetPredicate(request));
        var page = pageCache.GetSingleDataById(response.PageID);
        if (page != null)
            response.Page = page;
        return (response.IsNotNullOrEmpty())
            ? ResponseHelper.SuccessResponse(response)
            : ResponseHelper.ErrorResponse<PageObjectModel>(ExceptionMessageHelper.DataNotFound);
    }
    private Expression<Func<PageObjectModel, bool>> GetPredicate(PageObjectFilterModel request)
    {
        var predicate = PredicateBuilderHelper.False<PageObjectModel>();
        if (!request.ActivationStatus.HasValue)
            predicate = predicate.And(q => q.ActivationStatus == (int)ActivationStatusEnum.Active);
        else
            predicate = predicate.And(q => q.ActivationStatus == request.ActivationStatus);

        predicate = predicate.And(q => q.PageObjectName.ToLower().Contains(request.PageObjectName.ToLower()));
        return predicate;
    }
    private bool DataValidation(string pageObjectName, int? id)
    {
        var data = cache.GetAllData();
        var predicate = PredicateBuilderHelper.False<PageObjectModel>();
        predicate = predicate.And(q => q.PageObjectName.ToLower() == pageObjectName.ToLower());
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
