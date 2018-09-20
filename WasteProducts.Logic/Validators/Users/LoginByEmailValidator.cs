using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Models.Users.WebUsers;

namespace WasteProducts.Logic.Validators.Users
{
    public class LoginByEmailValidator : AbstractValidator<LoginByEmail>
    {
        public LoginByEmailValidator()
        {
            //TODO fix validation
            RuleFor(x => x.Email).NotEmpty().Matches(@"\d{13}");
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
