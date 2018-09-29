using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WasteProducts.Logic.Common.Models.Users.WebUsers;

namespace WasteProducts.Web.Validators.Users
{
    public class LoginByEmailValidator : AbstractValidator<LoginByEmail>
    {
        private const int _minPasswordLength = 6;
        private const int _maxPasswordLength = 12;

        public LoginByEmailValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress()
                .WithMessage("A valid email address is required.");

            RuleFor(x => x.Password).NotNull().NotEmpty()
                .MinimumLength(_minPasswordLength).MaximumLength(_maxPasswordLength)
                .WithMessage("Password cannot be empty and should " +
                $"be between {_minPasswordLength} and {_maxPasswordLength} characters length.");
        }
    }
}