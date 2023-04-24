using System.Linq.Expressions;

using MySampleFW.UserDomain.Services.Interfaces;

using MySampleFW.UserDomain.Repositories.CacheInterfaces;
using MySampleFW.UserDomain.Data.Interfaces;
using MySampleFW.Helper.Validations.Interfaces;
using MySampleFW.UserDomain.Libraries.Models;
using MyCore.Common.Base;
using MyCore.LogManager.ExceptionHandling;
using MySampleFW.Helper.Maps;
using MySampleFW.UserDomain.Libraries.Entities;
using MyCore.Common.Helper;
using MySampleFW.RoleDomain.Libraries.Models;
using MySampleFW.RoleDomain.Libraries.Entities;
using MySampleFW.RoleDomain.Repositores.Interfaces;
using MyCore.TokenManager;

namespace MySampleFW.UserDomain.Services.ServicesManager;
public class UserInfoServices : IUserInfoServices
{
    #region private
    private IUserInfoRepository repository;
    private IPagesRepository pagesRepository;
    private IUserInfoCache cache;
    private IValidateManager validate;
    #endregion

    #region Ctor
    public UserInfoServices(IUserInfoRepository _repository, IPagesRepository _pagesRepository, IUserInfoCache _cache
    , IValidateManager _validate)
    {
        repository = _repository;
        pagesRepository = _pagesRepository;
        cache = _cache;
        validate = _validate;
    }
    #endregion

    #region Methods
    public ResponseBase<UserInfoModel> Create(RequestBase<UserInfoCreateModel> request)
    {
        var rData = request.RequestData;
        if (!DataValidation(rData.UserName, rData.EMail, rData.GSM, null))
            return ResponseHelper.ErrorResponse<UserInfoModel>(ExceptionMessageHelper.IsInUse("UserInfo"));

        var entity = MapperInstance.Instance.Map<UserInfoCreateModel, UserInfoEntity>(rData);
        var userPassword = (rData.Password.IsNullOrEmpty())
            ? PasswordHelper.GeneratePassword()
            : rData.Password;

        entity.Password = PasswordHelper.HashPassword(userPassword);
        var validateResult = validate.UsersValidate(entity).Result;
        if (validateResult.Any())
            return ResponseHelper.ErrorResponse<UserInfoModel>(validateResult);

        var result = repository.Create(entity, request.RequestUserId);
        if (result.IsCompletedSuccessfully && !result.Id.IsNullOrLessOrEqToZero())
        {
            var returnModel = MapperInstance.Instance.Map<UserInfoEntity, UserInfoModel>(result.Result);
            cache.AddSingleData(returnModel);
            returnModel.Password = userPassword;
            return ResponseHelper.SuccessResponse(returnModel);
        }
        return ResponseHelper.ErrorResponse<UserInfoModel>(ExceptionMessageHelper.ProcessFailedResult);
    }
    public ResponseBase<UserInfoModel> Update(RequestBase<UserInfoUpdateModel> request)
    {
        var rData = request.RequestData;
        if (!DataValidation(rData.UserName, rData.EMail, rData.GSM, rData.ID))
            return ResponseHelper.ErrorResponse<UserInfoModel>(ExceptionMessageHelper.IsInUse("UserInfo"));

        var cachedData = cache.GetAllData();
        var fData = cachedData.FirstOrDefault(q => q.ID.Value == rData.ID);
        var entity = MapperInstance.Instance.Map<UserInfoUpdateModel, UserInfoEntity>(rData);
        var validateResult = validate.UsersValidate(entity).Result;
        if (validateResult.Any())
            return ResponseHelper.ErrorResponse<UserInfoModel>(ExceptionMessageHelper.IsInUse("UserInfo"));
        var result = repository.Update(entity, request.RequestUserId);

        if (result.IsCompleted)
        {
            var model = MapperInstance.Instance.Map<UserInfoEntity, UserInfoModel>(result.Result);
            cache.AddSingleData(model);
            return ResponseHelper.SuccessResponse(model);
        }
        return ResponseHelper.ErrorResponse<UserInfoModel>(ExceptionMessageHelper.ProcessFailedResult);
    }
    public ResponseBase<UserInfoModel> ChangePassword(RequestBase<UserInfoChangePasswordModel> request)
    {
        var rData = request.RequestData;
        var result = repository.GetSingleById(rData.Id);
        if (result.Result != null)
        {
            var hashPass = PasswordHelper.HashPassword(rData.NewPassword);
            var entity = result.Result;
            entity.Password = hashPass;
            var updateResult = repository.Update(entity, entity.CreatedBy);
            return (updateResult.IsCompletedSuccessfully)
                ? ResponseHelper.SuccessResponse(MapperInstance.Instance.Map<UserInfoEntity, UserInfoModel>(updateResult.Result))
                : ResponseHelper.ErrorResponse<UserInfoModel>(ExceptionMessageHelper.ProcessFailedResult, ResultEnum.Warning);
        }
        return ResponseHelper.ErrorResponse<UserInfoModel>(ExceptionMessageHelper.ProcessFailedResult, ResultEnum.Warning);
    }
    public ResponseBase<UserInfoModel> ChangeStatus(RequestBase<UserInfoStatusChangeModel> request)
    {
        var rData = request.RequestData;
        if (rData.ID.IsNullOrLessOrEqToZero())
            return ResponseHelper.ErrorResponse<UserInfoModel>(ExceptionMessageHelper.RequiredField("ID"));

        var predicate = PredicateBuilderHelper.False<UserInfoModel>();
        predicate = predicate.And(q => q.ID == rData.ID);
        predicate = predicate.And(q => q.ActivationStatus == rData.ActivationStatus);

        var model = cache.GetSingleDataByFilter(predicate);
        model.ActivationStatus = rData.ActivationStatus;
        var entity = MapperInstance.Instance.Map<UserInfoModel, UserInfoEntity>(model);
        var result = repository.Update(entity, request.RequestUserId);
        if (result.IsNotNullOrEmpty())
        {
            cache.ReFillCache();
            return ResponseHelper.SuccessResponse(model);
        }
        return ResponseHelper.ErrorResponse<UserInfoModel>(ExceptionMessageHelper.ProcessFailedResult);
    }
    public ResponseBase<IQueryable<UserInfoListModel>> GetDataByFilter(UserInfoFilterModel request)
    {
        var cachedData = cache.GetAllData();
        var pp = GetPredicate(request);
        var data = cachedData.Where(pp);
        //var data2 = cachedData.Where(q => q.UserGroup == request.UserGroup);
        if (!data.Any())
            return ResponseHelper.ErrorResponse<IQueryable<UserInfoListModel>>(ExceptionMessageHelper.DataNotFound);

        var response = MapperInstance.Instance.Map<List<UserInfoModel>, List<UserInfoListModel>>(data.ToList());
        return ResponseHelper.SuccessResponse(response.AsQueryable());
    }
    private Expression<Func<UserInfoModel, bool>> GetPredicate(UserInfoFilterModel request)
    {
        var predicate = PredicateBuilderHelper.True<UserInfoModel>();
        if (request.NameSurname.IsNotNullOrEmpty())
            predicate = predicate.And(q => q.NameSurname == request.NameSurname);
        if (request.UserName.IsNotNullOrEmpty())
            predicate = predicate.And(q => q.UserName == request.UserName);
        if (request.EMail.IsNotNullOrEmpty())
            predicate = predicate.And(q => q.EMail == request.EMail);
        if (request.UserGroup.HasValue)
            predicate = predicate.And(q => q.UserGroup == request.UserGroup);
        if (request.GSM.IsNotNullOrEmpty())
            predicate = predicate.And(q => q.GSM == request.GSM);
        return predicate;
    }
    public ResponseBase<UserInfoModel> GetSingleDataByFilter(UserInfoFilterModel request)
    {
        var data = cache.GetAllData();
        var response = data.FirstOrDefault(GetPredicate(request));
        return (response.IsNotNullOrEmpty())
                ? ResponseHelper.SuccessResponse(response)
                : ResponseHelper.ErrorResponse<UserInfoModel>(ExceptionMessageHelper.DataNotFound);
    }
    public ResponseBase<UserLoginResponseModel> UserLogin(UserLoginRequestModel request)
    {
        var user = repository.GetUserInfoLogin(request.LoginName, request.LoginName);
        if (user != null)
        {
            var hashPass = PasswordHelper.HashPassword(request.Password);
            if (user.Password == hashPass)
            {
                var userPages = pagesRepository.GetPagesByUserID(user.ID).Result;
                var UserToken = TokenHelper.GenerateToken(user.ID.ToString(), user.UserName, userPages.ToJson());
                var result = new UserLoginResponseModel
                {
                    ID = user.ID,
                    EMail = user.EMail,
                    GSM = user.GSM,
                    UserGroupID = user.UserGroup,
                    UserName = user.UserName,
                    UserType = user.UserType,
                    UserToken = UserToken,
                    LeftMenuList = GetLeftMenu(userPages)
                };
                return ResponseHelper.SuccessResponse(result);
            }
        }
        return ResponseHelper.ErrorResponse<UserLoginResponseModel>(ExceptionMessageHelper.LoginEror);
    }

    private ICollection<LeftMainMenuModel> GetLeftMenu(IQueryable<UserRolesPagesRepository> userPages)
    {
        var result = new List<LeftMainMenuModel>();
        var topMenuList = userPages.Where(q => q.PageLevel == 0).ToList();
        foreach (var item in topMenuList)
        {
            var subMenuList = userPages.Where(q => q.BindPageId == item.ID).Select(t => new LeftSubMenuModel
            {
                ID = t.ID,
                BindPageID = t.BindPageId,
                IconName = t.IconName,
                PageName = t.PageName,
                PageURL = t.PageURL
            });
            var model = (subMenuList.Any())
                ? new LeftMainMenuModel
                {
                    ID = item.ID,
                    IconName = item.IconName,
                    PageName = item.PageName,
                    PageURL = item.PageURL,
                    LeftSubMenuList = subMenuList.ToList()
                }
                : new LeftMainMenuModel();
            result.Add(model);
        }
        return result;
    }

    private bool DataValidation(string userName, string eMail, string gsm, int? id)
    {
        var data = cache.GetAllData();
        var predicate = PredicateBuilderHelper.False<UserInfoModel>();
        predicate = predicate.Or(q => q.UserName.ToLower() == userName.ToLower());
        predicate = predicate.Or(q => q.EMail.ToLower() == eMail.ToLower());
        predicate = predicate.Or(q => q.GSM.ToLower() == gsm.ToLower());
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
