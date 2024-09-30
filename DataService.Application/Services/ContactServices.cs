using AutoMapper;
using DataService.Domain;
using DataService.Repositories;

namespace DataService.Services
{
    public interface IContactService
    {
        Task<List<Contact>> GetContacts();
        Task AddContact(Contact contactDto);
        Task AddContactsBulk(List<Contact> contacts);
        Task<bool> UpdateContact(int id, Contact contactDto);
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

        public async Task AddContact(Contact contactDto)
        {
            var contact = _mapper.Map<Contact>(contactDto);
            await _contactRepository.AddContact(contact);
        }

        public async Task AddContactsBulk(List<Contact> contactsDto)
        {
            var contacts = _mapper.Map<List<Contact>>(contactsDto);
            await _contactRepository.AddContactsBulk(contacts);
        }

        public async Task<bool> UpdateContact(int id, Contact contactDto)
        {
            var contact = await _contactRepository.GetContactById(id);
            if (contact == null)
            {
                return false;
            }

            _mapper.Map(contactDto, contact);
            await _contactRepository.UpdateContact(contact);
            return true;
        }

        public async Task<bool> DeleteContact(int id)
        {
            return await _contactRepository.DeleteContact(id);
        }
    }
}
