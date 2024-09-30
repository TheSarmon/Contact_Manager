using MediatR;
using AutoMapper;
using DataService.Commands;
using DataService.Repositories;

namespace DataService.Handlers
{
    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, bool>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public UpdateContactCommandHandler(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetContactById(request.Id);
            if (contact == null)
            {
                return false;
            }

            _mapper.Map(request.Contact, contact);
            await _contactRepository.UpdateContact(contact);
            return true;
        }
    }
}
