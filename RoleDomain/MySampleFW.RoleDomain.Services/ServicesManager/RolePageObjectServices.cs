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
public class RolePageObjectServices : IRolePageObjectServices
{
    #region private
    private IRolePageObjectRepository RolePageObjectRepository;
    private IRolePageObjectCache cache;
    private IRolesCache rolesCache;
    private IPageObjectCache pageObjectCache;
    private IValidateManager validate;
    #endregion

    #region Ctor
    public RolePageObjectServices(
        IRolePageObjectRepository _RolePageObjectRepository,
        IRolePageObjectCache _cache,
        IRolesCache _rolesCache,
        IPageObjectCache _pageObjectCache,
        IValidateManager _validate)
    {
        RolePageObjectRepository = _RolePageObjectRepository;
        cache = _cache;
        rolesCache = _rolesCache;
        pageObjectCache = _pageObjectCache;
        validate = _validate;
    }
    #endregion

    #region Methods
    public ResponseBase<RolePageObjectModel> CreateOrUpdate(RequestBase<RolePageObjectCreateOrUpdateModel> request)
    {
        var rData = request.RequestData;
        var entity = MapperInstance.Instance.Map<RolePageObjectCreateOrUpdateModel, RolePageObjectEntity>(rData);

        var validateResult = validate.RolesValidate(entity).Result;
        if (validateResult.Any())
            return ResponseHelper.ErrorResponse<RolePageObjectModel>(validateResult);

        var result = (rData.ID.HasValue)
            ? RolePageObjectRepository.Create(entity, request.RequestUserId)
            : RolePageObjectRepository.Update(entity, request.RequestUserId);

        if (result.IsCompletedSuccessfully && !result.Id.IsNullOrLessOrEqToZero())
        {
            var response = MapperInstance.Instance.Map<RolePageObjectEntity, RolePageObjectModel>(result.Result);
            cache.AddSingleData(response);
            return ResponseHelper.SuccessResponse(response);
        }
        return ResponseHelper.ErrorResponse<RolePageObjectModel>(ExceptionMessageHelper.ProcessFailedResult);
    }
    public ResponseBase<IQueryable<RolePageObjectModel>> GetDataByFilter(RolePageObjectFilterModel request)
    {
        var data = cache.GetAllData();
        var response = data.Where(GetPredicate(request));
        return (response.IsNotNullOrEmpty())
            ? ResponseHelper.SuccessResponse(response)
            : ResponseHelper.ErrorResponse<IQueryable<RolePageObjectModel>>(ExceptionMessageHelper.DataNotFound);
    }
    public ResponseBase<RolePageObjectModel> GetSingleDataByFilter(RolePageObjectFilterModel request)
    {
        var data = cache.GetAllData();
        var response = data.FirstOrDefault(GetPredicate(request));
        var role = rolesCache.GetSingleDataById(request.RoleID);
        var pageObject = pageObjectCache.GetSingleDataById(request.PageObjectID);
        response.Role = role;
        response.PageObject = pageObject;
        return (response.IsNotNullOrEmpty())
            ? ResponseHelper.SuccessResponse(response)
            : ResponseHelper.ErrorResponse<RolePageObjectModel>(ExceptionMessageHelper.DataNotFound);
    }
    private Expression<Func<RolePageObjectModel, bool>> GetPredicate(RolePageObjectFilterModel request)
    {
        var predicate = PredicateBuilderHelper.False<RolePageObjectModel>();
        if (!request.ActivationStatus.HasValue)
            predicate = predicate.And(q => q.ActivationStatus == (int)ActivationStatusEnum.Active);
        else
            predicate = predicate.And(q => q.ActivationStatus == request.ActivationStatus);

        predicate = predicate.Or(q => q.RoleID == request.RoleID);
        predicate = predicate.Or(q => q.PageObjectID == request.PageObjectID);
        return predicate;
    }
    #endregion
}
