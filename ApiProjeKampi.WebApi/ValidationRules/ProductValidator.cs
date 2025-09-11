using ApiProjeKampi.WebApi.Entities;
using FluentValidation;

namespace ApiProjeKampi.WebApi.ValidationRules
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.ProductName).NotEmpty().WithMessage("Ürün Adını Boş Geçmeyin.");
            RuleFor(p => p.ProductName).MinimumLength(2).WithMessage("En az 2 karakter girişi yapın!");
            RuleFor(p => p.ProductName).MaximumLength(50).WithMessage("En Fazla 50 karakter veri girişi yapın.");

            RuleFor(p => p.Price).NotEmpty().WithMessage("Ürün Fiyatı Boş geçilemez.").GreaterThan(0).WithMessage("Ürün fiyatı Negatif olamaz")
                .LessThan(1000).WithMessage("Ürün Fiyatı Bu kadar yüksek olamaz. Girdiğiniz değeri kontrol edin.");

            RuleFor(p => p.ProductDescription).NotEmpty().WithMessage("Ürün Açıklaması Boş geçilemez.");

           
        }
    }
}
