﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using WasteProducts.Logic.Common.Models.Search;

namespace WasteProducts.Logic.Validators.Search
{
    class BoostedSearchQueryValidator : AbstractValidator<BoostedSearchQuery>
    {
        public BoostedSearchQueryValidator()
        {
            RuleFor(x => x.Query).NotEmpty();
            RuleFor(x => x.SearchableFields.Count).GreaterThan(0);
            RuleFor(x => x.BoostValues.Count).Equal(x => x.SearchableFields.Count);
        }
    }
}