using FluentValidation;
using WasteProducts.Logic.Common.Models.Barcods;

namespace WasteProducts.Logic.Validators.Barcods
{
    public class BarcodeValidator : AbstractValidator<Barcode>
    {
        public BarcodeValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Product.Name).NotEmpty().When(x => x.Product != null);
        }
    }
}
