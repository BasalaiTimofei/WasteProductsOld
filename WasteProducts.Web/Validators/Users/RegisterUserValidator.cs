using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WasteProducts.Web.Models.Users;

namespace WasteProducts.Web.Validators.Users
{
    public class RegisterUserValidator : AbstractValidator<RegisterUser>
    {
        private const int _minPasswordLength = 6;
        private const int _maxPasswordLength = 12;
        private const int _minUserNameLength = 4;
        private const int _maxUserNameLength = 16;

        public RegisterUserValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress()
                .WithMessage("A valid email address is required.");

            RuleFor(x => x.Password).NotNull().NotEmpty()
                .MinimumLength(_minPasswordLength).MaximumLength(_maxPasswordLength)
                .WithMessage("Password cannot be empty and should be " +
                $"between {_minPasswordLength} and {_maxPasswordLength} characters length.");

            RuleFor(x => x.UserName).NotNull().NotEmpty()
                .MinimumLength(_minUserNameLength).MaximumLength(_maxUserNameLength)
                .WithMessage("User Name cannot be empty and should be " +
                $"between {_minUserNameLength} and {_maxUserNameLength} characters length.");
        }
    }
}