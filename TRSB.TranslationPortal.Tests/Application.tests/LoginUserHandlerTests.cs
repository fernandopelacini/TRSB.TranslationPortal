using Application.Commands.LoginUserRequest;
using Application.Interfaces;
using Domain.Entities;
using Moq;

namespace TRSB.TranslationPortal.Tests.Application.tests
{
    public class LoginUserHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Return_Empty_When_User_Not_Found()
        {
            var repo = new Mock<IUserRepository>();
            var jwt = new Mock<IJwtService>();

            repo.Setup(r => r.GetUserForLoginAsync("tito", It.IsAny<CancellationToken>()))
                .ReturnsAsync((User?)null);

            var handler = new LoginUserHandler(repo.Object, jwt.Object);

            var cmd = new LoginUserCommand("tito", "pass123");

            var result = await handler.Handle(cmd, CancellationToken.None);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_When_Password_Invalid()
        {
            var repo = new Mock<IUserRepository>();
            var jwt = new Mock<IJwtService>();

            var user = new User
            {
                UserName = "tito",
                PasswordHash = [1],
                PasswordSalt = [2]
            };

            repo.Setup(r => r.GetUserForLoginAsync("tito", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            repo.Setup(r => r.VerifyPasswordHash("pass123", user.PasswordHash, user.PasswordSalt))
                .Returns(false);

            var handler = new LoginUserHandler(repo.Object, jwt.Object);

            var cmd = new LoginUserCommand("tito", "pass123");

            var result = await handler.Handle(cmd, CancellationToken.None);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public async Task Handle_Should_Return_Jwt_When_Login_Valid()
        {
            var repo = new Mock<IUserRepository>();
            var jwt = new Mock<IJwtService>();

            var user = new User
            {
                UserName = "carolina",
                PasswordHash = [1],
                PasswordSalt = [2]
            };

            repo.Setup(r => r.GetUserForLoginAsync("carolina", It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            repo.Setup(r => r.VerifyPasswordHash("pass123", user.PasswordHash, user.PasswordSalt))
                .Returns(true);

            jwt.Setup(j => j.GenerateToken(user)).Returns("TOKEN123");

            var handler = new LoginUserHandler(repo.Object, jwt.Object);

            var cmd = new LoginUserCommand("carolina", "pass123");

            var result = await handler.Handle(cmd, CancellationToken.None);

            Assert.Equal("TOKEN123", result);
        }
    }
}
