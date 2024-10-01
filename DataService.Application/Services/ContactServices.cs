using AutoMapper;
using DataService.Domain;
using DataService.Repositories;
using System.Diagnostics.Contracts;

namespace DataService.Services
{
    public interface IContactService
    {
        Task<List<Contact>> GetContacts();
        Task AddContact(Contact contact);
        Task AddContactsBulk(List<Contact> contacts);
        Task<bool> UpdateContact(int id, Contact contact);
        Task<bool> DeleteContact(int id);
    }

    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactService(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<List<Contact>> GetContacts()
        {
            var contacts = await _contactRepository.GetContacts();
            return _mapper.Map<List<Contact>>(contacts);
        }

        public async Task AddContact(Contact contact)
        {
            var newContact = _mapper.Map<Contact>(contact);
            await _contactRepository.AddContact(newContact);
        }

        public async Task AddContactsBulk(List<Contact> contacts)
        {
            var newContacts = _mapper.Map<List<Contact>>(contacts);
            await _contactRepository.AddContactsBulk(newContacts);
        }

        public async Task<bool> UpdateContact(int id, Contact contact)
        {
            var newContact = await _contactRepository.GetContactById(id);
            if (newContact == null)
            {
                return false;
            }

            _mapper.Map(newContact, contact);
            await _contactRepository.UpdateContact(contact);
            return true;
        }

        public async Task<bool> DeleteContact(int id)
        {
            return await _contactRepository.DeleteContact(id);
        }
    }
}
