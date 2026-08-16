using Application.Commands.CompleteTranslationRequest;
using Application.Commands.CreateTranslationRequest;
using Application.Queries.GetTranslationRequestById;
using Application.Queries.GetUserRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _04_Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TranslationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TranslationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTranslationRequestCommand cmd, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(cmd, cancellationToken);
            return id > 0 ? Ok(id) : BadRequest("Failed to create translation request.");

        }


        [HttpPut("complete")]
        public async Task<IActionResult> Complete(CompleteTranslationRequestCommand cmd, CancellationToken ct)
        {
            var result = await _mediator.Send(cmd, ct);
            return result ? Ok("Request completed") : BadRequest("Failed to complete request");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            int orgId = int.Parse(User.FindFirst("organizationId").Value);

            var result = await _mediator.Send(new GetTranslationRequestByIdQuery(id, orgId), ct);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRequests(int userId, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetUserRequestsQuery(userId), ct);
            return Ok(result);
        }

    }
}
