using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetUserRequests
{
    public class GetUserRequestsHandler : IRequestHandler<GetUserRequestsQuery, List<TranslationRequestDto>>
    {
        private readonly ITranslationRequestRepository _repo;
        public GetUserRequestsHandler(ITranslationRequestRepository repo)
        {
            _repo = repo;
        }
        public async Task<List<TranslationRequestDto>> Handle(GetUserRequestsQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetByUserIdAsync(request.UserId, cancellationToken);

            return list.Select(entity => new TranslationRequestDto(
            entity.Id,
            entity.UserId,
            entity.OrganizationId,
            entity.SourceText,
            entity.SourceLanguage,
            entity.TargetLanguage,
            entity.TranslatedText,
            entity.Status,
            entity.CreatedAt,
            entity.CompletedAt
        )).ToList();
        }
    }
}
