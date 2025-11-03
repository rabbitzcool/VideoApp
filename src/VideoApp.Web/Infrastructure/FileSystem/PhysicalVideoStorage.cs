using Microsoft.Extensions.Options;
using VideoApp.Web.Application.Abstractions;
using VideoApp.Web.Infrastructure.Configuration;

namespace VideoApp.Web.Infrastructure.FileSystem;

public sealed class PhysicalVideoStorage(
    IWebHostEnvironment webHostEnvironment,
    IOptions<MediaOptions> options) : IVideoStorage
{
    private readonly MediaOptions options = options.Value;

    public async Task SaveAsync(IFormFile file, CancellationToken cancellationToken)
    {
        string mediaRoot = Path.Combine(webHostEnvironment.WebRootPath, this.options.RelativePath);
        Directory.CreateDirectory(mediaRoot);

        string destPath = Path.Combine(mediaRoot, Path.GetFileName(file.FileName));
        await using FileStream stream = File.Create(destPath);

        await file.CopyToAsync(stream, cancellationToken);
    }
}

