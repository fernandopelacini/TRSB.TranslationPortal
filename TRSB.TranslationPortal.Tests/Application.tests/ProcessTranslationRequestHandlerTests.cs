using Application.Commands.ProcessTranslationRequest;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Moq;

namespace TRSB.TranslationPortal.Tests.Application.tests
{
    public class ProcessTranslationRequestHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Set_Status_To_Completee_And_Save_TranslatedText()
        {
            // Arrange
            var repo = new Mock<ITranslationRequestRepository>();
            var engine = new Mock<ITranslationEngine>();

            engine.Setup(e => e.Translate("hello this is text in xunit")).Returns("HELLO THIS IS A TEST IN XUNIT");

            var selector = new TranslationEngineSelector(new[] { engine.Object });

            var request = new TranslationRequest
            {
                Id = 1,
                SourceText = "hello this is text in xunit",
                Status = TranslationStatus.Soumise
            };

            repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(request);

            var handler = new ProcessTranslationRequestHandler(repo.Object, selector);

            // Act
            await handler.Handle(new ProcessTranslationRequestCommand(1), CancellationToken.None);

            // Assert
            Assert.Equal(TranslationStatus.Completee, request.Status);
            Assert.Equal("HELLO THIS IS A TEST IN XUNIT", request.TranslatedText);
            Assert.NotNull(request.CompletedAt);

            repo.Verify(r => r.UpdateAsync(request, It.IsAny<CancellationToken>()), Times.Exactly(2)); //2 times 1 saved is for setting in progress and the 2nd to completed
        }

        [Fact]
        public async Task Handle_Should_Do_Nothing_When_Request_Not_Found()
        {
            // Arrange
            var repo = new Mock<ITranslationRequestRepository>();
            var selector = new TranslationEngineSelector(new ITranslationEngine[0]);

            repo.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((TranslationRequest?)null);

            var handler = new ProcessTranslationRequestHandler(repo.Object, selector);

            // Act
            await handler.Handle(new ProcessTranslationRequestCommand(99), CancellationToken.None);

            // Assert
            repo.Verify(r => r.UpdateAsync(It.IsAny<TranslationRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
