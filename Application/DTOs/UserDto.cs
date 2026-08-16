
namespace Application.DTOs
{
    public record UserDto(
      int Id,
      string UserName,
      string FullName,
      string Email,
      int OrganizationId
  );
}
