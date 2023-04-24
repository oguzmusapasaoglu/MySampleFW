using MySampleFW.UserDomain.Libraries.Entities;
using MySampleFW.Helper.Validations.Interfaces;

using FluentValidation;
using MySampleFW.RoleDomain.Libraries.Entities;
using MySampleFW.Validations.Helper;

namespace MySampleFW.Helper.Validations.Helper;

public class ValidateManager : IValidateManager
{
    #region Private
    private IValidator<UserInfoEntity> userInfoValidator;
    private IValidator<UsersRolesEntity> userRoleValidator;

    private IValidator<RolesEntity> rolesValidator;
    private IValidator<RolePageEntity> rolePageValidator;
    private IValidator<RolePageObjectEntity> rolePageObjectValidator;
    private IValidator<PagesEntity> pagesValidator;
    private IValidator<PageObjectEntity> pageObjectValidator;
    #endregion

    #region Contructor
    public ValidateManager(
        IValidator<UserInfoEntity> _userInfoValidator,
        IValidator<UsersRolesEntity> _userRoleValidator,
        IValidator<RolesEntity> _rolesValidator,
        IValidator<RolePageEntity> _rolePageValidator,
        IValidator<RolePageObjectEntity> _rolePageObjectValidator,
        IValidator<PagesEntity> _pagesValidator,
        IValidator<PageObjectEntity> _pageObjectValidator)
    {
        #region Users
        userInfoValidator = _userInfoValidator;
        userRoleValidator = _userRoleValidator;
        #endregion

        #region Roles
        rolesValidator = _rolesValidator;
        rolePageValidator = _rolePageValidator;
        rolePageObjectValidator = _rolePageObjectValidator;
        pagesValidator = _pagesValidator;
        pageObjectValidator = _pageObjectValidator;
        #endregion
    }
    #endregion

    #region User
    public async Task<List<string>> UsersValidate(UserInfoEntity entity)
    {
        var validResult = await userInfoValidator.ValidateAsync(entity);
        if (!validResult.IsValid)
            return ValidatorHelper.GetErrors(validResult.Errors);
        return new List<string>();
    }
    public async Task<List<string>> UsersValidate(UsersRolesEntity entity)
    {
        var validResult = await userRoleValidator.ValidateAsync(entity);
        if (!validResult.IsValid)
            return ValidatorHelper.GetErrors(validResult.Errors);
        return new List<string>();
    }
    #endregion

    #region Role
    public async Task<List<string>> RolesValidate(RolesEntity entity)
    {
        var validResult = await rolesValidator.ValidateAsync(entity);
        if (!validResult.IsValid)
            return ValidatorHelper.GetErrors(validResult.Errors);
        return new List<string>();
    }
    public async Task<List<string>> RolesValidate(RolePageEntity entity)
    {
        var validResult = await rolePageValidator.ValidateAsync(entity);
        if (!validResult.IsValid)
            return ValidatorHelper.GetErrors(validResult.Errors);
        return new List<string>();
    }
    public async Task<List<string>> RolesValidate(RolePageObjectEntity entity)
    {
        var validResult = await rolePageObjectValidator.ValidateAsync(entity);
        if (!validResult.IsValid)
            return ValidatorHelper.GetErrors(validResult.Errors);
        return new List<string>();
    }
    public async Task<List<string>> RolesValidate(PagesEntity entity)
    {
        var validResult = await pagesValidator.ValidateAsync(entity);
        if (!validResult.IsValid)
            return ValidatorHelper.GetErrors(validResult.Errors);
        return new List<string>();
    }
    public async Task<List<string>> RolesValidate(PageObjectEntity entity)
    {
        var validResult = await pageObjectValidator.ValidateAsync(entity);
        if (!validResult.IsValid)
            return ValidatorHelper.GetErrors(validResult.Errors);
        return new List<string>();
    }
    #endregion
}
