using MediatR;

namespace Application.Commands.CreateTranslationRequest
{
    public class CreateTranslationRequestCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public string SourceText { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public List<string> Languages { get; set; } = [];

    }
}
