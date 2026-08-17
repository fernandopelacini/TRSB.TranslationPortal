using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public async Task CreateUserAsync(User request, CancellationToken cancellationToken)
        {
            _db.Users.Add(request);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> UserExists(string username, string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            {
                return true;
            }

            var user = await _db.Users.AsNoTracking().Where(u => u.UserName == username || u.Email == email).FirstOrDefaultAsync(cancellationToken);

            if (user != null)
            {
                return true;
            }

            return false;
        }

        public async Task<User?> GetUserForLoginAsync(string identifier, CancellationToken cancellationToken)
        {
            return await _db.Users
               .AsNoTracking()
               .Include(o => o.Organization)
               .Where(u => u.UserName == identifier || u.Email == identifier)
               .FirstOrDefaultAsync(cancellationToken);
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(passwordHash);
        }
        public async Task<List<User>> GetUsersByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken)
        {
            return await _db.Users
               .AsNoTracking()
               .Where(u => u.OrganizationId == organizationId)
               .ToListAsync(cancellationToken);
        }
    }
}
