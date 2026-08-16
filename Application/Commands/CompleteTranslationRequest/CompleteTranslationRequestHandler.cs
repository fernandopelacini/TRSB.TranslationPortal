using Application.Interfaces;
using Domain.Enums;
using MediatR;

namespace Application.Commands.CompleteTranslationRequest
{
    public class CompleteTranslationRequestHandler : IRequestHandler<CompleteTranslationRequestCommand, bool>
    {
        private readonly ITranslationRequestRepository _repo;
        public CompleteTranslationRequestHandler(ITranslationRequestRepository repo)
        {
            _repo = repo;
        }
        public async Task<bool> Handle(CompleteTranslationRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.RequestId, cancellationToken);
            if (entity == null)
                return false;

            entity.TranslatedText = request.TranslatedText;
            entity.Status = TranslationStatus.Completee;
            entity.CompletedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(entity, cancellationToken);

            return true;
        }
    }
}


