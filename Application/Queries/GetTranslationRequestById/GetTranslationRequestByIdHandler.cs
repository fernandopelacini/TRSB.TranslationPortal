using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetTranslationRequestById
{
    public class GetTranslationRequestByIdHandler : IRequestHandler<GetTranslationRequestByIdQuery, TranslationRequestDto>
    {
        private readonly ITranslationRequestRepository _repo;
        public GetTranslationRequestByIdHandler(ITranslationRequestRepository repo)
        {
            _repo = repo;
        }
        public async Task<TranslationRequestDto?> Handle(GetTranslationRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.RequestId, cancellationToken);

            if (entity == null)
                return null;

            //**Isolation des organisations** : un utilisateur ne doit en aucun cas pouvoir accéder aux demandes d'une autre organisation.
            if (entity.OrganizationId != request.OrganizationId)
                return null;

            return new TranslationRequestDto(
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
            );
        }
    }
}
