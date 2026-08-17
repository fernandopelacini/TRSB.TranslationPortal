using Application.Interfaces;
using Application.Services;
using Domain.Enums;
using MediatR;

namespace Application.Commands.ProcessTranslationRequest
{
    public class ProcessTranslationRequestHandler : IRequestHandler<ProcessTranslationRequestCommand>
    {
        private readonly ITranslationRequestRepository _repo;
        private readonly TranslationEngineSelector _selector;

        public ProcessTranslationRequestHandler(ITranslationRequestRepository repo, TranslationEngineSelector selector)
        {
            _repo = repo;
            _selector = selector;
        }

        public async Task Handle(ProcessTranslationRequestCommand cmd, CancellationToken cancellationToken)
        {
            
            var request = await _repo.GetByIdAsync(cmd.requestid, cancellationToken);
            if (request == null)
                return;

            request.Status = TranslationStatus.EnTraitement;
            request.ProcessingStartedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(request, cancellationToken);

            //// Just to simulate the translation process as the two engine options runs too fast.
            ////Not safe and will brake in a prod enviroment.
            //await Task.Delay(TimeSpan.FromSeconds(10), CancellationToken.None);

            var engine = _selector.SelectEngine();

            //Translatoin itselff
            request.TranslatedText = engine.Translate(request.SourceText);

            //Set as completed.
            request.Status = TranslationStatus.Completee;
            request.CompletedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(request, cancellationToken);
        }
    }
}
