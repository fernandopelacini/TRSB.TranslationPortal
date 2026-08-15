using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User
{
        public int Id { get; set; }
        [StringLength(15)]
        public string UserName { get; set; } = "";
        [StringLength(50)]
        public string FullName { get; set; } = "";
        [StringLength(80)]
        public string Email { get; set; } = "";
        public byte[] PasswordHash { get; set; } = [];
        public byte[] PasswordSalt { get; set; } = [];
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;

    }
}
