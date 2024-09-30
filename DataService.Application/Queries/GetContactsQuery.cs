using MediatR;
using DataService.Domain;

namespace DataService.Queries
{
    public class GetContactsQuery : IRequest<List<Contact>>
    {
    }
}
