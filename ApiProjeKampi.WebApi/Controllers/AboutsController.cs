using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.AboutDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutsController : ControllerBase
    {
        public readonly ApiContext _context;
        public readonly IMapper _mapper;
        public readonly IValidator<CreateAboutDto> _createValidator;
        public readonly IValidator<UpdateAboutDto> _updateValidator;

        public AboutsController(ApiContext context, IMapper mapper, IValidator<CreateAboutDto> createvalidator, IValidator<UpdateAboutDto> updatevalidator)
        {
            _context = context;
            _mapper = mapper;
            _createValidator = createvalidator;
            _updateValidator = updatevalidator;
        }

        [HttpGet]
        public IActionResult AboutList()
        {
            var value = _context.Abouts.ToList();
            return Ok(value);
        }

        [HttpPost]
        public IActionResult CreateAbout(CreateAboutDto createAboutDto)
        {
            var validationResult = _createValidator.Validate(createAboutDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(a => a.ErrorMessage));
            }
            else
            {
                var value = _mapper.Map<About>(createAboutDto);
                _context.Abouts.Add(value);
                _context.SaveChanges();
                return Ok("Hakkımızda Başaraıyla Eklenmiştir.");
            }

        }

        [HttpDelete]
        public IActionResult DeleteAbout(int id)
        {
            var value = _context.Abouts.Find(id);
            _context.Abouts.Remove(value);
            _context.SaveChanges();
            return Ok("Hakkımızda Başarıyla Silinmiştir.");
        }

        [HttpGet("GetAbout")]
        public IActionResult GetAbout(int id)
        {
            var value = _context.Abouts.Find(id);
            return Ok(value);
        }

        [HttpPut]
        public IActionResult UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            var validationResult = _updateValidator.Validate(updateAboutDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(a => a.ErrorMessage));
            }
            else
            {
                var value = _mapper.Map<About>(updateAboutDto);
                _context.Abouts.Update(value);
                _context.SaveChanges();
                return Ok("Hakkımızda Başarıyla Güncellenmiştir.");
            }

        }
    }
}
