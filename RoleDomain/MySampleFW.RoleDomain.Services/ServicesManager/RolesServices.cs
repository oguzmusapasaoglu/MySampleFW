using MyCore.Common.Base;
using System.Linq.Expressions;
using MySampleFW.RoleDomain.Libraries.Models;
using MySampleFW.RoleDomain.Services.Interfaces;
using MySampleFW.RoleDomain.Libraries.Entities;
using MySampleFW.RoleDomain.Services.CacheInterfaces;
using MySampleFW.Helper.Validations.Interfaces;
using MyCore.LogManager.ExceptionHandling;
using MySampleFW.Helper.Maps;
using MySampleFW.RoleDomain.Repositores.Interfaces;
using MyCore.Common.Helper;

namespace MySampleFW.RoleDomain.Services.ServicesManager;
public class RolesServices : IRolesServices
{
    #region private
    private IRolesRepository rolesRepository;
    private IRolesCache cache;
    private IRolePageObjectCache rolePageObjectCache;
    private IValidateManager validate;
    #endregion

    #region Ctor
    public RolesServices(
        IRolesRepository _rolesRepository,
        IRolesCache _cache,
        IRolePageObjectCache _rolePageObjectCache,
        IValidateManager _validate)
    {
        rolesRepository = _rolesRepository;
        cache = _cache;
        rolePageObjectCache = _rolePageObjectCache;
        validate = _validate;
    }
    #endregion

    #region Methods
    public ResponseBase<RolesModel> CreateOrUpdate(RequestBase<RolesCreateOrUpdateModel> request)
    {
        var rData = request.RequestData;
        if (!DataValidation(rData.RoleName, rData.ID))
            return ResponseHelper.ErrorResponse<RolesModel>(ExceptionMessageHelper.IsInUse("Roles"));

        var entity = MapperInstance.Instance.Map<RolesCreateOrUpdateModel, RolesEntity>(rData);
        var validateResult = validate.RolesValidate(entity).Result;
        if (validateResult.Any())
            return ResponseHelper.ErrorResponse<RolesModel>(validateResult);

        var result = rData.ID.HasValue
            ? rolesRepository.Update(entity, request.RequestUserId)
            : rolesRepository.Create(entity, request.RequestUserId);

        if (result.IsCompletedSuccessfully && !result.Id.IsNullOrLessOrEqToZero())
        {
            var returnModel = MapperInstance.Instance.Map<RolesEntity, RolesModel>(result.Result);
            cache.AddSingleData(returnModel);
            return ResponseHelper.SuccessResponse(returnModel);
        }
        return ResponseHelper.ErrorResponse<RolesModel>(ExceptionMessageHelper.ProcessFailedResult);
    }
    public ResponseBase<RolesModel> ChangeStatus(RequestBase<RolesStatusChangeModel> request)
    {
        var rData = request.RequestData;
        if (rData.ID.IsNullOrLessOrEqToZero())
            return ResponseHelper.ErrorResponse<RolesModel>(ExceptionMessageHelper.RequiredField("ID"), ResultEnum.Warning);

        var predicate = PredicateBuilderHelper.False<RolesModel>();
        predicate = predicate.And(q => q.ID == rData.ID);
        predicate = predicate.And(q => q.ActivationStatus == rData.ActivationStatus);

        var model = cache.GetSingleDataByFilter(predicate);
        model.ActivationStatus = rData.ActivationStatus;
        var entity = MapperInstance.Instance.Map<RolesModel, RolesEntity>(model);
        var result = rolesRepository.Update(entity, request.RequestUserId);
        if (result.IsNotNullOrEmpty())
        {
            cache.ReFillCache();
            return ResponseHelper.SuccessResponse(model);
        }
        return ResponseHelper.ErrorResponse<RolesModel>(ExceptionMessageHelper.ProcessFailedResult);
    }
    public ResponseBase<IQueryable<RolesModel>> GetDataByFilter(RolesFilterModel request)
    {
        var data = cache.GetAllData();
        var response = data.Where(GetPredicate(request));
        return (response != null)
            ? ResponseHelper.SuccessResponse(response)
            : ResponseHelper.ErrorResponse<IQueryable<RolesModel>>(ExceptionMessageHelper.DataNotFound);
    }
    public ResponseBase<RolesModel> GetSingleDataByFilter(RolesFilterModel request)
    {
        var data = cache.GetAllData();
        var response = data.FirstOrDefault(GetPredicate(request));
        var rolePageObject = rolePageObjectCache.GetDataByRoleId(response.ID.Value);
        response.RolePageObjects = rolePageObject;
        return (response != null)
            ? ResponseHelper.SuccessResponse(response)
            : ResponseHelper.ErrorResponse<RolesModel>(ExceptionMessageHelper.DataNotFound);
    }
    private Expression<Func<RolesModel, bool>> GetPredicate(RolesFilterModel request)
    {
        var predicate = PredicateBuilderHelper.False<RolesModel>();
        if (!request.ActivationStatus.HasValue)
            predicate = predicate.And(q => q.ActivationStatus == (int)ActivationStatusEnum.Active);
        else
            predicate = predicate.And(q => q.ActivationStatus == request.ActivationStatus);

        predicate = predicate.And(q => q.RoleName.ToLower().Contains(request.RoleName.ToLower()));
        return predicate;
    }
    private bool DataValidation(string roleName, int? id)
    {
        var data = cache.GetAllData();
        var predicate = PredicateBuilderHelper.False<RolesModel>();
        predicate = predicate.And(q => q.RoleName.ToLower() == roleName.ToLower());
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
