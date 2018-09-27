﻿using FluentValidation;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Validators.Groups
{
    public class GroupValidation : AbstractValidator<Group>
    {
        public GroupValidation()
        {
            RuleFor(x => x.AdminId)
            .NotNull();
            RuleFor(x => x.AdminId)
            .NotNull();

            RuleFor(x => x.Name)
            .NotNull()
            .Length(4, 50);

            RuleFor(x => x.Information)
                .Length(5, 255);

            RuleFor(x => x.GroupBoards)
                .Null();
            RuleFor(x => x.GroupUsers)
                .Null();
        }
    }
}
