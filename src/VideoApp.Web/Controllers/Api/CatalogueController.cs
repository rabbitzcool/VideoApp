using MediatR;
using Microsoft.AspNetCore.Mvc;
using VideoApp.Web.Application.Videos.List;
using VideoApp.Web.Controllers.Api.Extensions;
using VideoApp.Web.Entities;
using VideoApp.Web.Shared;

namespace VideoApp.Web.Controllers.Api;

[ApiController]
[Route("api/catalogue")]
public class CatalogueController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        ApiResult<IReadOnlyList<VideoItem>> result = await this.mediator.Send(new GetCatalogueQuery(), cancellationToken);
        return result.ToActionResult(this);
    }
}
