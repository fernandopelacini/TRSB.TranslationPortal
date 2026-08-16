using Application.DTOs;
using Application.Interfaces;
using MediatR;

namespace Application.Queries.GetOrganizationUsers
{
    public class GetOrganizationUsersHandler : IRequestHandler<GetOrganizationUsersQuery, List<UserDto>>
    {
        private readonly IUserRepository _repo;
        public GetOrganizationUsersHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<UserDto>> Handle(GetOrganizationUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _repo.GetUsersByOrganizationIdAsync(request.OrganizationId, cancellationToken);

            return users.Select(u => new UserDto(
                u.Id,
                u.UserName,
                u.FullName,
                u.Email,
                u.OrganizationId))
                .ToList();
        }
    }
}
