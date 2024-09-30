using MediatR;
using DataService.Commands;
using DataService.Repositories;

namespace DataService.Handlers
{
    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, bool>
    {
        private readonly IContactRepository _contactRepository;

        public DeleteContactCommandHandler(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<bool> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
        {
            return await _contactRepository.DeleteContact(request.Id);
        }
    }
}
