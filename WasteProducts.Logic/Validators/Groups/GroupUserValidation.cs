using FluentValidation;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Validators.Groups
{
    public class GroupUserValidation : AbstractValidator<GroupUser>
    {
        public GroupUserValidation()
        {
            RuleFor(x => x.GroupId)
            .NotNull();
            RuleFor(x => x.UserId)
            .NotNull();
        }
    }
}
