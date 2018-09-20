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
        public NewPasswordValidator()
        {
            RuleFor(x => x.Password).NotNull().NotEmpty();
        }
    }
}