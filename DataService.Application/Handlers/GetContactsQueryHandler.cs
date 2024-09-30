using MediatR;
using AutoMapper;
using DataService.Domain;
using DataService.Queries;
using DataService.Repositories;

namespace DataService.Handlers
{
    public class GetContactsQueryHandler : IRequestHandler<GetContactsQuery, List<Contact>>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public GetContactsQueryHandler(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<List<Contact>> Handle(GetContactsQuery request, CancellationToken cancellationToken)
        {
            var contacts = await _contactRepository.GetContacts();
            return _mapper.Map<List<Contact>>(contacts);
        }
    }
}
