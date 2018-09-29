using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WasteProducts.Web.Models.Users;

namespace WasteProducts.Web.Validators.Users
{
    public class ChangePasswordValidator : AbstractValidator<ChangePassword>
    {
        private const int _minPasswordLength = 6;
        private const int _maxPasswordLength = 12;

        public ChangePasswordValidator()
        {
            RuleFor(x => x.NewPassword).NotNull().NotEmpty()
                .NotEqual(a => a.OldPassword).WithMessage("New password shouldn't match old password. Password cannot be empty and should " +
                $"be between {_minPasswordLength} and {_maxPasswordLength} characters length.")
                .MinimumLength(_minPasswordLength).MaximumLength(_maxPasswordLength);

            RuleFor(x => x.OldPassword).NotNull().NotEmpty()
                .MinimumLength(_minPasswordLength).MaximumLength(_maxPasswordLength)
                .WithMessage("Make sure old password is correct. Password cannot be empty and should " +
                $"be between {_minPasswordLength} and {_maxPasswordLength} characters length.");
        }
    }
}