using Application.Commands.CompleteTranslationRequest;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Moq;

namespace TRSB.TranslationPortal.Tests.Application.tests
{
    public class CompleteTranslationRequestHandlerTests
{
        [Fact]
        public async Task Handle_Should_Return_False_When_Request_Not_Found()
        {
            var repo = new Mock<ITranslationRequestRepository>();
            var selector = new TranslationEngineSelector(new ITranslationEngine[0]);

            repo.Setup(r => r.GetByIdAsync(99, It.IsAny<CancellationToken>()))
                .ReturnsAsync((TranslationRequest?)null);

            var handler = new CompleteTranslationRequestHandler(repo.Object, selector);

            var cmd = new CompleteTranslationRequestCommand(99, "hello");

            var result = await handler.Handle(cmd, CancellationToken.None);

            Assert.False(result);
        }

        [Fact]
        public async Task Handle_Should_Complete_Request()
        {
            var repo = new Mock<ITranslationRequestRepository>();
            var engine = new Mock<ITranslationEngine>();

            engine.Setup(e => e.Translate("hello")).Returns("BONJOUR");

            var selector = new TranslationEngineSelector(new[] { engine.Object });

            var entity = new TranslationRequest
            {
                Id = 1,
                SourceText = "hello",
                Status = TranslationStatus.EnTraitement
            };

            repo.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);

            var handler = new CompleteTranslationRequestHandler(repo.Object, selector);

            var cmd = new CompleteTranslationRequestCommand(1, "hello");

            var result = await handler.Handle(cmd, CancellationToken.None);

            Assert.True(result);
            Assert.Equal("BONJOUR", entity.TranslatedText);
            Assert.Equal(TranslationStatus.Completee, entity.Status);
            Assert.NotNull(entity.CompletedAt);

            repo.Verify(r => r.UpdateAsync(entity, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
