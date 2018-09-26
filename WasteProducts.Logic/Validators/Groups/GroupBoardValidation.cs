using FluentValidation;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Validators.Groups
{
    public class GroupBoardValidation : AbstractValidator<GroupBoard>
    {
        public GroupBoardValidation()
        {
            RuleFor(x => x.GroupId)
            .NotNull();
            RuleFor(x => x.CreatorId)
            .NotNull();

            RuleFor(x => x.Name)
            .NotNull()
            .Length(4, 50);

            RuleFor(x => x.Information)
                .Length(5, 255);

            RuleFor(x => x.GroupProductComments)
                .Null();
            RuleFor(x => x.GroupProducts)
                .Null();
        }
    }
}
