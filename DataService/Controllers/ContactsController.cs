using Microsoft.AspNetCore.Mvc;
using DataService.Services;
using DataService.Dtos;

namespace DataService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            var contacts = await _contactService.GetContacts();
            return Ok(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] ContactDto contactDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (contactDto == null)
            {
                return BadRequest("Invalid contact data.");
            }

            await _contactService.AddContact(contactDto);
            return Ok("Contact added successfully.");
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddContactsBulk([FromBody] List<ContactDto> contacts)
        {
            if (contacts == null || !contacts.Any())
            {
                return BadRequest("No contacts provided.");
            }

            await _contactService.AddContactsBulk(contacts);
            return Ok("Contacts added successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] ContactDto contactDto)
        {
            if (contactDto == null)
            {
                return BadRequest("Invalid contact data.");
            }

            var result = await _contactService.UpdateContact(id, contactDto);
            if (!result)
            {
                return NotFound("Contact not found.");
            }

            return Ok("Contact updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var result = await _contactService.DeleteContact(id);
            if (!result)
            {
                return NotFound("Contact not found.");
            }

            return Ok("Contact deleted successfully.");
        }
    }
}
