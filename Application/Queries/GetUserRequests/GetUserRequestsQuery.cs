using Application.DTOs;
using MediatR;

namespace Application.Queries.GetUserRequests
{
    public record GetUserRequestsQuery(int UserId) : IRequest<List<TranslationRequestDto>>;
}
