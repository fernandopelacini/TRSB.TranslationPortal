using Application.Interfaces;
using MediatR;

namespace Application.Commands.LoginUserRequest 
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        public LoginUserHandler(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }
        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserForLoginAsync(request.Identifier, cancellationToken);

            if (user == null)
                return string.Empty;

            if (!_userRepository.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return string.Empty;

            return _jwtService.GenerateToken(user);
        }
    }
}
