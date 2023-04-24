using MySampleFW.UserDomain.Libraries.Entities;

using FluentValidation;

using MyCore.LogManager.ExceptionHandling;

namespace MySampleFW.Helper.Validations.UserValidator;

public class UserInfoValidator : AbstractValidator<UserInfoEntity>
{
    public UserInfoValidator()
    {
        RuleFor(x => x.NameSurname)
            .NotEmpty().WithMessage(ExceptionMessageHelper.RequiredField("Name Surname"))
            .MaximumLength(250).WithMessage(ExceptionMessageHelper.LengthError("Name Surname", 250));

        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage(ExceptionMessageHelper.RequiredField("User Name"))
            .MaximumLength(150).WithMessage(ExceptionMessageHelper.LengthError("User Name", 150));

        RuleFor(x => x.EMail)
            .NotEmpty().WithMessage(ExceptionMessageHelper.RequiredField("EMail"))
            .EmailAddress().WithMessage(ExceptionMessageHelper.EmailNotValid);

        RuleFor(x => x.GSM)
            .NotEmpty().WithMessage(ExceptionMessageHelper.RequiredField("GSM"))
            .MaximumLength(50).WithMessage(ExceptionMessageHelper.LengthError("GSM", 50));

        RuleFor(x => x.UserGroup)
            .NotNull().WithMessage(ExceptionMessageHelper.RequiredField("User Group"));

        RuleFor(x => x.UserType)
            .NotEmpty().WithMessage(ExceptionMessageHelper.RequiredField("User Type"));
    }
}
