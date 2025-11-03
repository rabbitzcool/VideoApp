using MediatR;
using Microsoft.AspNetCore.Mvc;
using VideoApp.Web.Application.Videos.Upload;
using VideoApp.Web.Controllers.Api.Extensions;
using VideoApp.Web.Shared;

namespace VideoApp.Web.Controllers.Api;

[ApiController]
[Route("api/upload")]
public class UploadsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator mediator = mediator;
    private const long MegaBytes = 1024 * 1024;
    private const long MaxFileSizeBytes = 200 * MegaBytes; // 200 MB

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] IFormFileCollection files, CancellationToken cancellationToken)
    {
        long totalRequestBytes = files.Sum(f => f.Length);
        if (totalRequestBytes > MaxFileSizeBytes)
        {
            return ApiResult.Failure(Errors.PayloadTooLarge(
                "Upload_TooLarge",
                $"The total upload size exceeds the maximum allowed size of {MaxFileSizeBytes / MegaBytes} MB."))
                .ToActionResult(this);
        }

        ApiResult result = await this.mediator.Send(new UploadVideosCommand(files), cancellationToken);
        return result.ToActionResult(this);
    }
}
