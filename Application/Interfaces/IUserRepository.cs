
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
{
        Task CreateUserAsync(User request, CancellationToken cancellationToken);
        Task<bool> UserExists (string username, string email, CancellationToken cancellationToken);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        Task<User?> GetUserForLoginAsync(string identifier, CancellationToken cancellationToken);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
       Task<List<User>> GetUsersByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken);

    }
}
