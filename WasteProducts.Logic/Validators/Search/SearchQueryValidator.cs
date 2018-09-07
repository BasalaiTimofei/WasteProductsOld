using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace WasteProducts.Logic.Common.Models.Search
{
    class SearchQueryValidator : AbstractValidator<SearchQuery>
    {
        public SearchQueryValidator()
        {
            RuleFor(x => x.Query).NotEmpty();
            RuleFor(x => x.SearchableFields.Count).GreaterThan(0);
        }
    }
}
