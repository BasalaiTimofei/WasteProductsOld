
using FluentValidation;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Validators.Groups
{
    public class GroupProductValidation : AbstractValidator<GroupProduct>
    {
        public GroupProductValidation()
        {
            RuleFor(x => x.GroupBoardId)
            .NotNull();
            RuleFor(x => x.ProductId)
            .NotNull();

            RuleFor(x => x.Information)
                .Length(0, 255);
        }
    }
}
