using Domain.Entities;

namespace Application.Interfaces
{
    public interface ITranslationRequestRepository
    {
        Task AddAsync(TranslationRequest request, CancellationToken cancellationToken);
        Task UpdateAsync(TranslationRequest request, CancellationToken cancellationToken);
        Task<TranslationRequest?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<TranslationRequest>> GetByUserIdAsync(int userId, CancellationToken cancellationToken);
    }
}
