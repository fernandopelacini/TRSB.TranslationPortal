using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Organization> Organizations { get; set; } = null!;
        public DbSet<TranslationRequest> TranslationRequests { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>().HasData(
                new Organization { Id = 1, Name = "Alpha Traductions" },
                new Organization { Id = 2, Name = "Bêta Légal" }
                );


            modelBuilder.Entity<TranslationRequest>()
                .HasOne(tr => tr.User)
                .WithMany()
                .HasForeignKey(tr => tr.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                modelBuilder.Entity<TranslationRequest>()
                .HasOne(tr => tr.Organization)
                .WithMany()
                .HasForeignKey(tr => tr.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
