using Application.DTOs;
using MediatR;

namespace Application.Queries.GetOrganizationUsers
{
    public record GetOrganizationUsersQuery(int OrganizationId) : IRequest<List<UserDto>>;
}
