using MediatR;
using AutoMapper;
using DataService.Commands;
using DataService.Domain;
using DataService.Repositories;

namespace DataService.Handlers
{
    public class AddContactCommandHandler : IRequestHandler<AddContactCommand>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public AddContactCommandHandler(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(AddContactCommand request, CancellationToken cancellationToken)
        {
            var contact = _mapper.Map<Contact>(request.Contact);
            await _contactRepository.AddContact(contact);
            return Unit.Value;
        }
    }
}
