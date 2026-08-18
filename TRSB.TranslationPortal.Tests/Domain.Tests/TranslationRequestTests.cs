using Domain.Entities;

namespace TRSB.TranslationPortal.Tests.Domain.Tests
{
    public class TranslationRequestTests
    {
        [Fact]
        public void New_Request_Should_Have_Default_Values()
        {
            var r = new TranslationRequest();

            Assert.Equal("", r.SourceText);
            Assert.Equal("", r.SourceLanguage);
            Assert.Equal("", r.TargetLanguage);
            Assert.Null(r.TranslatedText);
            Assert.Null(r.CompletedAt);
            Assert.Null(r.ProcessingStartedAt);
        }
    }
}
