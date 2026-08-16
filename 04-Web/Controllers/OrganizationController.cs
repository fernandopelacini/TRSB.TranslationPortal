using Microsoft.AspNetCore.Mvc;
using MediatR;
using Application.Queries.GetOrganizationUsers;
using Microsoft.AspNetCore.Authorization;

namespace _04_Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrganizationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetUsers(int id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetOrganizationUsersQuery(id), ct);
            return Ok(result);
        }
    }
}
