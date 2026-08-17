
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class TranslationRequest
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public Organization Organization { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string SourceText { get; set; } = "";
        [StringLength(30)]
        public string SourceLanguage { get; set; } = "";
        [StringLength(30)]
        public string TargetLanguage { get; set; } = "";
        public TranslationStatus Status { get; set; }
        public string? TranslatedText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? ProcessingStartedAt { get; set; }
    }
}
