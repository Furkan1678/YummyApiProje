using ApiProjeKampi.WebApi.Dtos.AboutDtos;
using FluentValidation;

namespace ApiProjeKampi.WebApi.ValidationRules
{
    public class UpdateAboutDtoValidator : AbstractValidator<UpdateAboutDto>
    {
        public UpdateAboutDtoValidator()
        {
            RuleFor(a => a.Title).NotEmpty().WithMessage("UHakkımızda Başlığı Boş geçilemez.").MinimumLength(2).WithMessage("Başlık En Az 2 karakter Uzunluğunda Olması Gerekmektedir.").MaximumLength(15).WithMessage("En fazla 15 karakter Uzunluğunda Olması Gerekmektedir.");
            RuleFor(a => a.Description).NotEmpty().WithMessage("THakkımızda Açıklaması Boş geçilemez.").MinimumLength(15).WithMessage("Açıklama En Az 15 karakter Uzunluğunda Olması Gerekmektedir.").MaximumLength(300).WithMessage("En fazla 300 karakter Uzunluğunda Olması Gerekmektedir.");
            RuleFor(a => a.ImageUrl).NotEmpty().WithMessage("UHakkımızda Resim Boş geçilemez.");
            RuleFor(a => a.VideoCoverImageUrl).NotEmpty().WithMessage("UHakkımızda Video Resmi Boş geçilemez.");
            RuleFor(a => a.VideoCoverUrl).NotEmpty().WithMessage("UHakkımızda Video Boş geçilemez.");
        }
    }
}
