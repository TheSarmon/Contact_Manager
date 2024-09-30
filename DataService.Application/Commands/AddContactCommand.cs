using MediatR;
using DataService.Domain;

namespace DataService.Commands
{
    public class AddContactCommand : IRequest
    {
        public Contact Contact { get; set; }

        public AddContactCommand(Contact contact)
        {
            Contact = contact;
        }
    }
}
