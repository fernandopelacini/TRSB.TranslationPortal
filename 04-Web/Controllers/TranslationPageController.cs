using Application.Commands.CreateTranslationRequest;
using Application.Queries.GetTranslationRequestById;
using Application.Queries.GetUserRequests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace _04_Web.Controllers
{
    [Authorize]
    public class TranslationPageController : Controller
    {
        private readonly IMediator _mediator;
        public TranslationPageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> MyRequests(CancellationToken ct)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);

            var requests = await _mediator.Send(new GetUserRequestsQuery(userId), ct);

            return View(requests);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateTranslationRequestCommand
            {
                Languages =
                [
                    "English",
                    "French",
                    "Spanish",
                    "German",
                    "Italian"
                    ]
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTranslationRequestCommand cmd, CancellationToken ct)
        {
            cmd.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty);
            cmd.OrganizationId = int.Parse(User.FindFirst("organizationId")?.Value ?? "0");

            var id = await _mediator.Send(cmd, ct);

            return RedirectToAction("Details", new { id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            int orgId = int.Parse(User.FindFirst("organizationId")?.Value ?? "0");

            var request = await _mediator.Send(new GetTranslationRequestByIdQuery(id, orgId), ct);

            if (request == null)
                return NotFound();

            return View(request);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ForbiddenDemo()
        {
            return Forbid(); // 403
        }
    }
}
