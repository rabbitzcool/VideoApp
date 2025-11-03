using MediatR;
using Microsoft.Extensions.Options;
using VideoApp.Web.Application.Abstractions;
using VideoApp.Web.Infrastructure.Configuration;
using VideoApp.Web.Shared;

namespace VideoApp.Web.Application.Videos.Upload;

public sealed class UploadVideosCommandHandler(
    IOptions<MediaOptions> opts,
    IVideoStorage storage) : IRequestHandler<UploadVideosCommand, ApiResult>
{
    private readonly IOptions<MediaOptions> opts = opts;
    private readonly IVideoStorage storage = storage;

    public async Task<ApiResult> Handle(UploadVideosCommand request, CancellationToken cancellationToken)
    {
        if (request.Files is null || request.Files.Count == 0)
        {
            return ApiResult.Failure(Errors.BadRequest("upload.empty", "No files selected."));
        }

        long maxBytes = this.opts.Value.UploadLimitMB * 1024L * 1024L;

        foreach (IFormFile file in request.Files)
        {
            if (!Path.GetExtension(file.FileName).Equals(".mp4", StringComparison.OrdinalIgnoreCase))
            {
                return ApiResult.Failure(Errors.BadRequest("upload.ext", "Only .mp4 files are allowed."));
            }

            if (file.Length > maxBytes)
            {
                return ApiResult.Failure(Errors.PayloadTooLarge("upload.size", $"File exceeds {this.opts.Value.UploadLimitMB} MB limit."));
            }

            await this.storage.SaveAsync(file, cancellationToken);
        }

        return ApiResult.Success();
    }
}