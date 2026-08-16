using Domain.Enums;

namespace Application.DTOs
{
    public record TranslationRequestDto(
        int Id,
        int UserId,
        int OrganizationId,
        string SourceText,
        string SourceLanguage,
        string TargetLanguage,
        string? TranslatedText,
        TranslationStatus Status,
        DateTime CreatedAt,
        DateTime? CompletedAt
    );
}
