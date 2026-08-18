using Application.Commands.RegisterUserRequest;
using Application.Interfaces;
using Domain.Entities;
using Moq;

namespace TRSB.TranslationPortal.Tests.Application.tests
{
    public class RegisterUserHandlerTests
{
        [Fact]
        public async Task Handle_Should_Return_False_When_User_Already_Exists()
        {
            var repo = new Mock<IUserRepository>();

            repo.Setup(r => r.UserExists("juan", "juancito@mail.com", It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = new RegisterUserHandler(repo.Object);

            var cmd = new RegisterUserCommand("juan", "Juan Perez", "juancito@mail.com", "pass123");

            var result = await handler.Handle(cmd, CancellationToken.None);

            Assert.False(result);
            repo.Verify(r => r.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_Should_Create_User_When_Not_Exists()
        {
            var repo = new Mock<IUserRepository>();

            repo.Setup(r => r.UserExists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            byte[] hash = [1, 2, 3];
            byte[] salt = [4, 5, 6];

            repo.Setup(r => r.CreatePasswordHash("pass123", out hash, out salt));

            var handler = new RegisterUserHandler(repo.Object);

            var cmd = new RegisterUserCommand("juan", "Juan Perez", "juancito@mail.com", "pass123");

            var result = await handler.Handle(cmd, CancellationToken.None);

            Assert.True(result);

            repo.Verify(r => r.CreateUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
