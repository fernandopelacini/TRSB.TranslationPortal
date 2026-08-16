using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TranslationRequestRepository : ITranslationRequestRepository
    {
        private readonly AppDbContext _db;
        public TranslationRequestRepository(AppDbContext db)
        {
            _db = db;
        }
        public async Task AddAsync(TranslationRequest request, CancellationToken cancellationToken)
        {
            _db.TranslationRequests.Add(request);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<TranslationRequest?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _db.TranslationRequests
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(TranslationRequest request, CancellationToken cancellationToken)
        {
            _db.TranslationRequests.Update(request);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<TranslationRequest>> GetByUserIdAsync(int userId, CancellationToken cancellationToken)
        {
            return await _db.TranslationRequests
                .AsNoTracking()
                .Where(r => r.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}
