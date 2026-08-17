using Domain.Entities;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace TRSB.TranslationPortal.Tests.Infrastructure.Tests
{
    public class TranslationRequestRepositoryTests
    {
        private AppDbContext CreateDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddAsync_Should_Save_Request()
        {
            var db = CreateDb();
            var repo = new TranslationRequestRepository(db);

            var r = new TranslationRequest { Id = 1, SourceText = "hello, this is an xunit test" };

            await repo.AddAsync(r, CancellationToken.None);

            Assert.Equal(1, db.TranslationRequests.Count());
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Request()
        {
            var db = CreateDb();
            var repo = new TranslationRequestRepository(db);

            var r = new TranslationRequest { Id = 1, SourceText = "hello, this is an xunit test" };
            db.TranslationRequests.Add(r);
            db.SaveChanges();

            var result = await repo.GetByIdAsync(1, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal("hello, this is an xunit test", result!.SourceText);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Request()
        {
            var db = CreateDb();
            var repo = new TranslationRequestRepository(db);

            var r = new TranslationRequest { Id = 1, SourceText = "hello, this is an xunit test" };
            db.TranslationRequests.Add(r);
            db.SaveChanges();

            r.SourceText = "updated";

            await repo.UpdateAsync(r, CancellationToken.None);

            Assert.Equal("updated", db.TranslationRequests.First().SourceText);
        }
    }
}
