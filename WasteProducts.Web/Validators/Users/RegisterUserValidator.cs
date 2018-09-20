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
        public RegisterUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email address is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User Name cannot be empty");
        }
    }
}