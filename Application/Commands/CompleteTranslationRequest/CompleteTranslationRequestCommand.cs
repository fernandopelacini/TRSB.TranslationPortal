using MediatR;

namespace Application.Commands.CompleteTranslationRequest
{
    public record CompleteTranslationRequestCommand(int RequestId, string SourceText) : IRequest<bool> 
    {
    }
}
