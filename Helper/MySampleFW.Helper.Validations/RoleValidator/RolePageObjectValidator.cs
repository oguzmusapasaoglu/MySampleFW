using MySampleFW.RoleDomain.Libraries.Entities;

using FluentValidation;

using MyCore.LogManager.ExceptionHandling;

namespace MySampleFW.Helper.Validations.RoleValidator;

public class RolePageObjectValidator : AbstractValidator<RolePageObjectEntity>
{
    public RolePageObjectValidator()
    {
        RuleFor(x => x.RoleID)
          .NotEmpty().WithMessage(ExceptionMessageHelper.RequiredField("Role"));

        RuleFor(x => x.PageObjectID)
          .NotEmpty().WithMessage(ExceptionMessageHelper.RequiredField("Page Object"));
    }
}
