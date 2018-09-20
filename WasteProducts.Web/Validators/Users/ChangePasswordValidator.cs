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
        public ChangePasswordValidator()
        {
            RuleFor(x => x.NewPassword).NotNull().NotEmpty().NotEqual(a => a.OldPassword).WithMessage("New password shouldn't match old password.");
            RuleFor(x => x.OldPassword).NotNull().NotEmpty();
        }
    }
}