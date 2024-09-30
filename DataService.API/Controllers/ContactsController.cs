using Microsoft.AspNetCore.Mvc;
using MediatR;
using DataService.Commands;
using DataService.Queries;
using DataService.Domain;

namespace DataService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            var contacts = await _mediator.Send(new GetContactsQuery());
            return Ok(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact([FromBody] Contact contactDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (contactDto == null)
            {
                return BadRequest("Invalid contact data.");
            }

            await _mediator.Send(new AddContactCommand(contactDto));
            return Ok("Contact added successfully.");
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddContactsBulk([FromBody] List<Contact> contacts)
        {
            if (contacts == null || !contacts.Any())
            {
                return BadRequest("No contacts provided.");
            }

            await _mediator.Send(new AddContactsBulkCommand(contacts));
            return Ok("Contacts added successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, [FromBody] Contact contact)
        {
            if (contact == null)
            {
                return BadRequest("Invalid contact data.");
            }

            var result = await _mediator.Send(new UpdateContactCommand(id, contact));
            if (result.Equals(false))// ЗВЕРНУТИ УВАГУ
            {
                return NotFound("Contact not found.");
            }

            return Ok("Contact updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var result = await _mediator.Send(new DeleteContactCommand(id));
            if (!result)
            {
                return NotFound("Contact not found.");
            }

            return Ok("Contact deleted successfully.");
        }
    }
}