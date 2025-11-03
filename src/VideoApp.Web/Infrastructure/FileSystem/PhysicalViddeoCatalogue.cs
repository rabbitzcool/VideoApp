using System.Collections.ObjectModel;
using Microsoft.Extensions.Options;
using VideoApp.Web.Application.Abstractions;
using VideoApp.Web.Entities;
using VideoApp.Web.Infrastructure.Configuration;

namespace VideoApp.Web.Infrastructure.FileSystem;

public sealed class PhysicalVideoCatalogue(
    IWebHostEnvironment webHostEnvironment,
    IOptions<MediaOptions> options) : IVideoCatalogue
{
    private const long KiloByte = 1024;
    private readonly IWebHostEnvironment webHostEnvironment = webHostEnvironment;
    private readonly MediaOptions options = options.Value;

    public Task<IReadOnlyList<VideoItem>> ListAsync(CancellationToken cancelationToken)
    {
        string mediaRoot = Path.Combine(this.webHostEnvironment.WebRootPath, this.options.RelativePath);
        Directory.CreateDirectory(mediaRoot);

        ReadOnlyCollection<VideoItem> items = Directory.EnumerateFiles(mediaRoot, "*.mp4")
            .Select(p => new FileInfo(p))
            .OrderBy(f => f.Name)
            .Select(f => new VideoItem(
                FileName: f.Name,
                SizeKiloBytes: f.Length / KiloByte,
                Url: $"/{this.options.RelativePath}/{Uri.EscapeDataString(f.Name)}"))
            .ToList()
            .AsReadOnly();

        return Task.FromResult<IReadOnlyList<VideoItem>>(items);
    }
}

