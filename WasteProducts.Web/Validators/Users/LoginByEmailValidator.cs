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
        public LoginByEmailValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email address is required.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty");
        }
    }
}