using MediatR;
using AutoMapper;
using DataService.Commands;
using DataService.Domain;
using DataService.Repositories;

namespace DataService.Handlers
{
    public class AddContactsBulkCommandHandler : IRequestHandler<AddContactsBulkCommand>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public AddContactsBulkCommandHandler(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddContactsBulkCommand request, CancellationToken cancellationToken)
        {
            var contacts = _mapper.Map<List<Contact>>(request.Contacts);
            await _contactRepository.AddContactsBulk(contacts);
            return Unit.Value;
        }
    }
}
