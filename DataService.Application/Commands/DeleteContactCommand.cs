using MediatR;

namespace DataService.Commands
{
    public class DeleteContactCommand : IRequest<bool>
    {
        public int Id { get; }

        public DeleteContactCommand(int id)
        {
            Id = id;
        }
    }
}
