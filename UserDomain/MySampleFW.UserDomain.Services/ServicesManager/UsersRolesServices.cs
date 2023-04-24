using MySampleFW.UserDomain.Services.Interfaces;
using MySampleFW.UserDomain.Data.Interfaces;
using MySampleFW.Helper.Validations.Interfaces;
using MySampleFW.UserDomain.Libraries.Models;
using MyCore.Common.Base;
using MyCore.LogManager.ExceptionHandling;
using MySampleFW.Helper.Maps;
using MySampleFW.UserDomain.Libraries.Entities;
using MyCore.Common.Helper;
using System.Linq.Expressions;

namespace MySampleFW.UserDomain.Services.ServicesManager;

public class UsersRolesServices : IUsersRolesServices
{
    #region private
    private IUsersRolesRepository repository;
    private IValidateManager validate;
    #endregion

    #region Ctor
    public UsersRolesServices(IUsersRolesRepository _repository, IValidateManager _validate)
    {
        repository = _repository;
        validate = _validate;
    }
    #endregion

    #region Methods
    public ResponseBase<UsersRolesModel> CreateOrUpdate(RequestBase<UsersRolesCreateOrUpdateModel> request)
    {
        var rData = request.RequestData;
        if (DataValidation(rData.UserID, rData.RoleID, rData.ID))
            return ResponseHelper.ErrorResponse<UsersRolesModel>(ExceptionMessageHelper.IsInUse("User Role"));

        var entity = MapperInstance.Instance.Map<UsersRolesCreateOrUpdateModel, UsersRolesEntity>(rData);
        var validateResult = validate.UsersValidate(entity).Result;
        if (validateResult.Any())
            return ResponseHelper.ErrorResponse<UsersRolesModel>(validateResult);

        var result = (rData.ID.HasValue)
            ? repository.Create(entity, request.RequestUserId)
            : repository.Update(entity, request.RequestUserId);
        if (result.IsCompletedSuccessfully && !result.Id.IsNullOrLessOrEqToZero())
        {
            var returnModel = MapperInstance.Instance.Map<UsersRolesEntity, UsersRolesModel>(result.Result);
            return ResponseHelper.SuccessResponse(returnModel);
        }
        return ResponseHelper.ErrorResponse<UsersRolesModel>(ExceptionMessageHelper.ProcessFailedResult);
    }
    public ResponseBase<dynamic> BulkCreate(RequestBase<List<UsersRolesBulkCreateModel>> request)
    {
        var rData = request.RequestData;
        var entity = MapperInstance.Instance.Map<List<UsersRolesBulkCreateModel>, List<UsersRolesEntity>>(rData);
        var result = repository.BulkCreate(entity);
        return (result.Result)
            ? ResponseHelper.SuccessResponse<dynamic>(result.Result)
            : ResponseHelper.ErrorResponse<dynamic>(ExceptionMessageHelper.ProcessFailedResult);
    }
    public ResponseBase<IQueryable<UsersRolesModel>> GetDataByFilter(UsersRolesFilterModel request)
    {
        var predicate = GetPredicate(request);
        var data = repository.GetAllByFilter(predicate).Result;
        if (data.IsNullOrEmpty())
        {
            var returnModel = MapperInstance.Instance.Map<IQueryable<UsersRolesEntity>, IQueryable<UsersRolesModel>>(data);
            return ResponseHelper.SuccessResponse(returnModel);
        }
        return ResponseHelper.ErrorResponse<IQueryable<UsersRolesModel>>(ExceptionMessageHelper.DataNotFound);
    }
    public ResponseBase<IQueryable<UsersRolesModel>> GetDataByRoleID(UsersRolesFilterModel request)
    {
        var predicate = GetPredicate(request);
        var data = repository.GetAllByFilter(predicate).Result;
        if (data.IsNullOrEmpty())
        {
            var returnModel = MapperInstance.Instance.Map<IQueryable<UsersRolesEntity>, IQueryable<UsersRolesModel>>(data);
            return ResponseHelper.SuccessResponse(returnModel);
        }
        return ResponseHelper.ErrorResponse<IQueryable<UsersRolesModel>>(ExceptionMessageHelper.DataNotFound);
    }
    public ResponseBase<IQueryable<UsersRolesModel>> GetDataByUserID(UsersRolesFilterModel request)
    {
        var predicate = GetPredicate(request);
        var data = repository.GetAllByFilter(predicate).Result;
        if (data.IsNullOrEmpty())
        {
            var returnModel = MapperInstance.Instance.Map<IQueryable<UsersRolesEntity>, IQueryable<UsersRolesModel>>(data);
            return ResponseHelper.SuccessResponse(returnModel);
        }
        return ResponseHelper.ErrorResponse<IQueryable<UsersRolesModel>>(ExceptionMessageHelper.DataNotFound);
    }
    private Expression<Func<UsersRolesEntity, bool>> GetPredicate(UsersRolesFilterModel request)
    {
        var predicate = PredicateBuilderHelper.False<UsersRolesEntity>();
        if (!request.ActivationStatus.HasValue)
            predicate = predicate.And(q => q.ActivationStatus == (int)ActivationStatusEnum.Active);
        else
            predicate = predicate.And(q => q.ActivationStatus == request.ActivationStatus);
        if (request.UserID.HasValue)
            predicate = predicate.And(q => q.UserID == request.UserID);
        if (request.RoleID.HasValue)
            predicate = predicate.And(q => q.RoleID == request.RoleID);
        return predicate;
    }
    private bool DataValidation(int userID, int roleID, int? id = null)
    {
        var predicate = PredicateBuilderHelper.False<UsersRolesEntity>();
        predicate = predicate.And(q => q.ActivationStatus == (int)ActivationStatusEnum.Active);
        predicate = predicate.And(q => q.UserID == userID);
        predicate = predicate.And(q => q.RoleID == roleID);
        var data = repository.GetSingleByFilter(predicate).ConfigureAwait(false).GetAwaiter().GetResult();
        if (data != null && data.ID == id)
            return false;
        return true;
    }

    #endregion
}
