using Application.Interfaces;
using Application.Services;
using Domain.Enums;
using MediatR;

namespace Application.Commands.CompleteTranslationRequest
{
    public class CompleteTranslationRequestHandler : IRequestHandler<CompleteTranslationRequestCommand, bool>
    {
        private readonly ITranslationRequestRepository _repo;
        private readonly TranslationEngineSelector _engineSelector;
        public CompleteTranslationRequestHandler(ITranslationRequestRepository repo, TranslationEngineSelector engineSelector)
        {
            _repo = repo;
            _engineSelector = engineSelector;
        }
        public async Task<bool> Handle(CompleteTranslationRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.RequestId, cancellationToken);
            if (entity == null)
                return false;

            var engine = _engineSelector.SelectEngine();

            entity.TranslatedText =engine.Translate(request.SourceText);
            entity.Status = TranslationStatus.Completee;
            entity.CompletedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(entity, cancellationToken);

            return true;
        }
    }
}


