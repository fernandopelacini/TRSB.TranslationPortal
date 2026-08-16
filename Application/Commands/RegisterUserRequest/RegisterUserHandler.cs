using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Commands.RegisterUserRequest
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        public RegisterUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _userRepository.UserExists(request.username, request.email, cancellationToken))
            {
                return false;
            }
            
            _userRepository.CreatePasswordHash(request.password, out byte[] passwordHash, out byte[] passwordSalt);

            var entity = new User
            {
                UserName = request.username,
                FullName = request.fullname,
                Email = request.email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                OrganizationId = new Random().Next(1, 3) //1 = Alpha, 2 = Beta
            };

            await _userRepository.CreateUserAsync(entity, cancellationToken);
            return true;
        }
    }
}
