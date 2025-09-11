using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.ContactDtos;
using ApiProjeKampi.WebApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ApiContext _context;

        public ContactsController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ContactList()
        {
            var values = _context.Contacts.ToList();
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateContact(CreateContactDto createContactDto)
        {
            Contact contact = new Contact();
            contact.Email = createContactDto.Email;
            contact.Adress = createContactDto.Adress;      //mapper kullanmasaydık böyle yeni bir nesne oluştururak dto'ları çekecektik.
            contact.Phone = createContactDto.Phone;
            contact.MapLocation = createContactDto.MapLocation;
            contact.OpenHours = createContactDto.OpenHours;
            _context.Contacts.Add(contact);
            _context.SaveChanges();
            return Ok("Contact Ekleme İşlemi Başarılı");
        }

        [HttpDelete]
        public IActionResult DeleteContact(int id)
        {
           var value = _context.Contacts.Find(id);
            _context.Contacts.Remove(value);
            _context.SaveChanges();
            return Ok("Contact Silme İşlemi Başarılı.");
        }

        [HttpGet("GetContact")]
        public IActionResult GetContact(int id)
        {
            var value = _context.Contacts.Find(id);
            return Ok(value);
        }

        [HttpPut]
        public IActionResult UpdateContact(UpdateContactDto updateContactDto) {
            Contact contact = new();
            contact.Email = updateContactDto.Email;                        
            contact.ContactId = updateContactDto.ContactId;
            contact.Adress = updateContactDto.Adress;
            contact.Phone = updateContactDto.Phone;
            contact.MapLocation= updateContactDto.MapLocation;  
            contact.OpenHours= updateContactDto.OpenHours;
            _context.Contacts.Update(contact);
            _context.SaveChanges();
            return Ok("Contact İşlemi Başarıyla Güncellendi.");
        
        }
            

    }
}
