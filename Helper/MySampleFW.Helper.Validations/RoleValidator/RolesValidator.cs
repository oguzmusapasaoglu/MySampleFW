﻿using MySampleFW.RoleDomain.Libraries.Entities;

using FluentValidation;

using MyCore.LogManager.ExceptionHandling;

namespace MySampleFW.Helper.Validations.RoleValidator
{
    public class RolesValidator : AbstractValidator<RolesEntity>
    {
        public RolesValidator()
        {
            RuleFor(x => x.RoleName)
                .NotEmpty().WithMessage(ExceptionMessageHelper.RequiredField("Role Name"))
                .MaximumLength(150).WithMessage(ExceptionMessageHelper.LengthError("Role Name", 150));

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage(ExceptionMessageHelper.RequiredField("Description"))
                .MaximumLength(500).WithMessage(ExceptionMessageHelper.LengthError("Description", 500));

        }
    }
}
