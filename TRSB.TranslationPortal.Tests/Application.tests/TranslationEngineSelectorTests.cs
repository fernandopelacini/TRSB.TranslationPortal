using Application.Interfaces;
using Application.Services;
using Moq;

namespace TRSB.TranslationPortal.Tests.Application.tests
{
    public class TranslationEngineSelectorTests
    {
        [Fact]
        public void SelectEngine_Should_Return_One_Of_The_Engines()
        {
            // Arrange
            var e1 = new Mock<ITranslationEngine>().Object;
            var e2 = new Mock<ITranslationEngine>().Object;
            var selector = new TranslationEngineSelector(new[] { e1, e2 });

            // Act
            var engine = selector.SelectEngine();

            // Assert
            Assert.Contains(engine, new[] { e1, e2 });
        }

        [Fact]
        public void SelectEngine_Should_Not_Return_Null()
        {
            var e1 = new Mock<ITranslationEngine>().Object;
            var selector = new TranslationEngineSelector(new[] { e1 });

            var engine = selector.SelectEngine();

            Assert.NotNull(engine);
        }
    }
}
