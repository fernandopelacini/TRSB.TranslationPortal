using Application.DTOs;
using MediatR;

namespace Application.Queries.GetTranslationRequestById
{
    public record GetTranslationRequestByIdQuery(int RequestId, int OrganizationId) : IRequest<TranslationRequestDto>;
}
