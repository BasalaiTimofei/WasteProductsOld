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
        private const int _minUserNameLength = 4;
        private const int _maxUserNameLength = 16;

        public UserNameModelValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty()
                .MinimumLength(_minUserNameLength).MaximumLength(_maxUserNameLength)
                .WithMessage("User Name cannot be empty and should be " +
                $"between {_minUserNameLength} and {_maxUserNameLength} characters length.");
        }
    }
}