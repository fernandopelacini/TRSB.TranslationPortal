using Application.Commands.CreateTranslationRequest;
using Application.Interfaces;
using Domain.Entities;
using Moq;

namespace TRSB.TranslationPortal.Tests.Application.tests
{
   public class CreateTranslationRequestHandlerTests
{
        [Fact]
        public async Task Handle_Should_Create_Request_With_Correct_Values()
        {
            var repo = new Mock<ITranslationRequestRepository>();

            repo.Setup(r => r.AddAsync(It.IsAny<TranslationRequest>(), It.IsAny<CancellationToken>()))
                .Callback<TranslationRequest, CancellationToken>((r, _) => r.Id = 10)
                .Returns(Task.CompletedTask);

            var handler = new CreateTranslationRequestHandler(repo.Object);

            var cmd = new CreateTranslationRequestCommand { 
                UserId = 5,
                OrganizationId = 2,
                SourceText = "hello",
                SourceLanguage = "EN",
                TargetLanguage = "FR",
                Languages = new List<string> { "EN", "FR", "ESP", "JPN", "ITA" }
            };

            var id = await handler.Handle(cmd, CancellationToken.None);

            Assert.Equal(10, id);

            repo.Verify(r => r.AddAsync(It.IsAny<TranslationRequest>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
