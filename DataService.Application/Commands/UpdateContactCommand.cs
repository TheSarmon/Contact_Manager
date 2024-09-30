using MediatR;
using DataService.Domain;

namespace DataService.Commands
{
    public class UpdateContactCommand :IRequest<bool>
    {
        public int Id { get; }
        public Contact Contact { get; }

        public UpdateContactCommand(int id, Contact contact)
        {
            Id = id;
            Contact = contact;
        }
    }
}
