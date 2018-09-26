using FluentValidation;
using WasteProducts.Logic.Common.Models.Groups;

namespace WasteProducts.Logic.Validators.Groups
{
    public class GroupCommentValidation : AbstractValidator<GroupComment>
    {
        public GroupCommentValidation()
        {
            RuleFor(x => x.GroupBoardId)
            .NotNull();
            RuleFor(x => x.CommentatorId)
            .NotNull();

            RuleFor(x => x.Comment)
                .Length(1, 255);
        }
    }
}
