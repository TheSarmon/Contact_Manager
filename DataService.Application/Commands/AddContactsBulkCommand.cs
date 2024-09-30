using MediatR;
using DataService.Domain;

namespace DataService.Commands
{
    public class AddContactsBulkCommand : IRequest
    {
        public List<Contact> Contacts { get; }

        public AddContactsBulkCommand(List<Contact> contacts)
        {
            Contacts = contacts;
        }
    }
}
