using MyCore.Common.Base;
using MyCore.Common.Helper;
using MyCore.LogManager.ExceptionHandling;
using MySampleFW.Helper.Maps;
using MySampleFW.Helper.Validations.Interfaces;
using MySampleFW.RoleDomain.Libraries.Entities;
using MySampleFW.RoleDomain.Libraries.Models;
using MySampleFW.RoleDomain.Repositores.Interfaces;
using MySampleFW.RoleDomain.Services.CacheInterfaces;
using MySampleFW.RoleDomain.Services.Interfaces;

using System.Linq.Expressions;

namespace MySampleFW.RoleDomain.Services.ServicesManager
{
    public class RolePageServices : IRolePageServices
    {
        #region private
        private IRolePageRepository repository;
        private IRolePageCache cache;
        private IValidateManager validate;
        #endregion

        #region Ctor
        public RolePageServices(
            IRolePageRepository _repository,
            IRolePageCache _cache,
            IValidateManager _validate)
        {
            repository = _repository;
            cache = _cache;
            validate = _validate;
        }
        #endregion

        #region Methods
        public ResponseBase<RolePageListModel> CreateOrUpdate(RequestBase<RolePageModel> request)
        {
            var rData = request.RequestData;
            var entity = MapperInstance.Instance.Map<RolePageModel, RolePageEntity>(rData);

            var validateResult = validate.RolesValidate(entity).Result;
            if (validateResult.Any())
                return ResponseHelper.ErrorResponse<RolePageListModel>(validateResult);

            var result = (rData.ID.HasValue)
                ? repository.Create(entity, request.RequestUserId)
                : repository.Update(entity, request.RequestUserId);

            if (result.IsCompletedSuccessfully && !result.Id.IsNullOrLessOrEqToZero())
            {
                var rolePageList = repository.GetSingleRolePageByID(result.Id);
                var returnModel = MapperInstance.Instance.Map<RolePageEntity, RolePageListModel>(rolePageList.Result);
                cache.AddSingleData(returnModel);
                return ResponseHelper.SuccessResponse(returnModel);
            }
            return ResponseHelper.ErrorResponse<RolePageListModel>(ExceptionMessageHelper.ProcessFailedResult);
        }
        public async Task<ResponseBase<IQueryable<RolePageListModel>>> GetDataByFilter(RolePageModel request)
        {
            var data = cache.GetAllData();
            var response = data.Where(GetPredicate(request));
            return (response != null)
                ? ResponseHelper.SuccessResponse(response)
                : ResponseHelper.ErrorResponse<IQueryable<RolePageListModel>>(ExceptionMessageHelper.DataNotFound);
        }
        public async Task<IQueryable<RolePageListModel>> GetRolePagesByPageID(int pageID)
        {
            var data = cache.GetAllData();
            var response = data.Where(q => q.PageID == pageID).AsQueryable();
            return response;
        }
        public async Task<IQueryable<RolePageListModel>> GetRolePagesByRoleID(int roleID)
        {
            var data = cache.GetAllData();
            var response = data.Where(q => q.RoleID == roleID).AsQueryable();
            return response;
        }
        public async Task<IQueryable<RolePageListModel>> GetRolePagesByUserID(int userID)
        {
            var data = await repository.GetRolePagesByUserID(userID);
            var returnModel = MapperInstance.Instance.Map<IQueryable<RolePageEntity>, IQueryable<RolePageListModel>>(data);
            return returnModel;
        }
        public async Task<ResponseBase<RolePageListModel>> GetSingleDataByID(int id)
        {
            var data = cache.GetAllData();
            var response = data.FirstOrDefault(q => q.ID == id);
            return ResponseHelper.SuccessResponse(response);
        }
        private Expression<Func<RolePageListModel, bool>> GetPredicate(RolePageModel request)
        {
            var predicate = PredicateBuilderHelper.False<RolePageListModel>();
            predicate = predicate.And(q => q.ActivationStatus == (int)ActivationStatusEnum.Active);
            predicate = predicate.Or(q => q.RoleID == request.RoleID);
            predicate = predicate.Or(q => q.PageID == request.PageID);
            return predicate;
        }
        #endregion
    }
}
