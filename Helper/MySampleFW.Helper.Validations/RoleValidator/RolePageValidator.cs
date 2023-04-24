using MySampleFW.RoleDomain.Libraries.Entities;

using FluentValidation;

using MyCore.LogManager.ExceptionHandling;

namespace MySampleFW.Helper.Validations.RoleValidator
{
    public class RolePageValidator : AbstractValidator<RolePageEntity>
    {
        public RolePageValidator()
        {
            RuleFor(x => x.RoleID)
              .NotEmpty().WithMessage(ExceptionMessageHelper.RequiredField("Role"));

            RuleFor(x => x.PageID)
              .NotEmpty().WithMessage(ExceptionMessageHelper.RequiredField("Page"));
        }
    }
}
