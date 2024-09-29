using DataService.Data;
using DataService.Models;
using Microsoft.EntityFrameworkCore;

namespace DataService.Repositories
{
    public interface IContactRepository
    {
        Task<List<Contact>> GetContacts();
        Task<Contact> GetContactById(int id);
        Task AddContact(Contact contact);
        Task AddContactsBulk(List<Contact> contacts);
        Task UpdateContact(Contact contact);
        Task<bool> DeleteContact(int id);
    }

    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Contact>> GetContacts()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task<Contact> GetContactById(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public async Task AddContact(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
        }

        public async Task AddContactsBulk(List<Contact> contacts)
        {
            _context.Contacts.AddRange(contacts);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateContact(Contact contact)
        {
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteContact(int id)
        {
            var contact = await GetContactById(id);
            if (contact == null)
            {
                return false;
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
