using MediatR;

namespace Application.Commands.LoginUserRequest
{
    //Identifier can be either username or email
    //2. **Ouvrir une session** avec le nom d'usager **ou** le courriel
    public record LoginUserCommand(string Identifier, string Password) : IRequest<string>;

}
