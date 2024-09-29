using DataService.Dtos;
using DataService.Models;
using DataService.Repositories;

namespace DataService.Services
{
    public interface IContactService
    {
        Task<List<Contact>> GetContacts();
        Task AddContact(ContactDto contactDto);
        Task AddContactsBulk(List<ContactDto> contacts);
        Task<bool> UpdateContact(int id, ContactDto contactDto);
        Task<bool> DeleteContact(int id);
    }

    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<List<Contact>> GetContacts()
        {
            return await _contactRepository.GetContacts();
        }

        public async Task AddContact(ContactDto contactDto)
        {
            var contact = new Contact
            {
                Name = contactDto.Name,
                DateOfBirth = contactDto.DateOfBirth,
                Married = contactDto.Married,
                Phone = contactDto.Phone,
                Salary = contactDto.Salary
            };

            await _contactRepository.AddContact(contact);
        }

        public async Task AddContactsBulk(List<ContactDto> contacts)
        {
            var contactEntities = contacts.Select(contact => new Contact
            {
                Name = contact.Name,
                DateOfBirth = contact.DateOfBirth,
                Married = contact.Married,
                Phone = contact.Phone,
                Salary = contact.Salary
            }).ToList();

            await _contactRepository.AddContactsBulk(contactEntities);
        }

        public async Task<bool> UpdateContact(int id, ContactDto contactDto)
        {
            var contact = await _contactRepository.GetContactById(id);
            if (contact == null)
            {
                return false;
            }

            contact.Name = contactDto.Name;
            contact.DateOfBirth = contactDto.DateOfBirth;
            contact.Married = contactDto.Married;
            contact.Phone = contactDto.Phone;
            contact.Salary = contactDto.Salary;

            await _contactRepository.UpdateContact(contact);
            return true;
        }

        public async Task<bool> DeleteContact(int id)
        {
            return await _contactRepository.DeleteContact(id);
        }
    }
}
