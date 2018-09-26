using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WasteProducts.Web.Models.Users;

namespace WasteProducts.Web.Validators.Users
{
    public class UserNameModelValidator : AbstractValidator<UserNameModel>
    {
        public UserNameModelValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty().WithMessage("User Name cannot be empty");
        }
    }
}