using ApiProjeKampi.WebApi.Dtos.AboutDtos;
using ApiProjeKampi.WebApi.Entities;
using FluentValidation;

namespace ApiProjeKampi.WebApi.ValidationRules
{
    public class CreateAboutDtoValidator : AbstractValidator<CreateAboutDto>
    {
        public CreateAboutDtoValidator()
        {
            RuleFor(a => a.Title).NotEmpty().WithMessage("Hakkımızda Başlığı Boş geçilemez.").MinimumLength(2).WithMessage("En Az 2 karakter Uzunluğunda Olması Gerekmektedir.").MaximumLength(15).WithMessage("En fazla 15 karakter Uzunluğunda Olması Gerekmektedir.");   
            RuleFor(a => a.Description).NotEmpty().WithMessage("Hakkımızda Açıklaması Boş geçilemez.").MinimumLength(15).WithMessage("En Az 15 karakter Uzunluğunda Olması Gerekmektedir.").MaximumLength(300).WithMessage("En fazla 300 karakter Uzunluğunda Olması Gerekmektedir.");
            RuleFor(a => a.ImageUrl).NotEmpty().WithMessage("Hakkımızda Resim Boş geçilemez.");   
            RuleFor(a => a.VideoCoverImageUrl).NotEmpty().WithMessage("Hakkımızda Video Resmi Boş geçilemez.");   
            RuleFor(a => a.VideoCoverUrl).NotEmpty().WithMessage("Hakkımızda Video Boş geçilemez.");   
        }
    }
}
