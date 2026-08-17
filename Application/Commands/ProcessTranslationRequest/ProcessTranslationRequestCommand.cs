using MediatR;

namespace Application.Commands.ProcessTranslationRequest
{
    public record ProcessTranslationRequestCommand(int requestid) : IRequest;

}
