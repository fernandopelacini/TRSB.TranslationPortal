using MediatR;
using Domain.Entities;
using Application.Interfaces;
using Domain.Enums;

namespace Application.Commands.CreateTranslationRequest
{
    public class CreateTranslationRequestHandler : IRequestHandler<CreateTranslationRequestCommand, int>
    {
        private readonly ITranslationRequestRepository _repo;
        public CreateTranslationRequestHandler(ITranslationRequestRepository repo)
        {
            _repo = repo;
        }
        public async Task<int> Handle(CreateTranslationRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = new TranslationRequest
            {
                UserId = request.UserId,
                OrganizationId = request.OrganizationId,
                SourceText = request.SourceText,
                SourceLanguage = request.SourceLanguage,
                TargetLanguage = request.TargetLanguage,
                Status = TranslationStatus.Soumise,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(entity, cancellationToken);
            return entity.Id;
        }
    }
}
