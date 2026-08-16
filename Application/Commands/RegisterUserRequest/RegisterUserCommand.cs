using MediatR;

namespace Application.Commands.RegisterUserRequest
{
    public record RegisterUserCommand(string username, string fullname, string email, string password, int organizationId) : IRequest<bool>
    {
    }
}
