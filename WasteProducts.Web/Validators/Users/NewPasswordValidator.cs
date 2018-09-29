using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WasteProducts.Web.Models.Users;

namespace WasteProducts.Web.Validators.Users
{
    public class NewPasswordValidator : AbstractValidator<NewPassword>
    {
        private const int _minPasswordLength = 6;
        private const int _maxPasswordLength = 12;

        public NewPasswordValidator()
        {
            RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(_minPasswordLength)
                .MaximumLength(_maxPasswordLength).WithMessage("Password cannot be empty and should be " +
                $"between {_minPasswordLength} and {_maxPasswordLength} characters length.");
        }
    }
}