using MySampleFW.UserDomain.Libraries.Entities;

using FluentValidation;

using MyCore.LogManager.ExceptionHandling;

namespace MySampleFW.Helper.Validations.UserValidator
{
    public class UsersRolesValidator : AbstractValidator<UsersRolesEntity>
    {
        public UsersRolesValidator()
        {
            RuleFor(x => x.RoleID)
           .NotEmpty().WithMessage(ExceptionMessageHelper.RequiredField("Role"));

            RuleFor(x => x.UserID)
           .NotEmpty().WithMessage(ExceptionMessageHelper.RequiredField("User"));
        }
    }
}
