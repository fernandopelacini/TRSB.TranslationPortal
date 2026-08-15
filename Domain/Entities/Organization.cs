
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Organization
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; } = "";
        public ICollection<User> Users { get; set; } = [];
    }
}
