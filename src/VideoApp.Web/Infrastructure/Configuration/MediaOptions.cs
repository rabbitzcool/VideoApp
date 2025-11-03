namespace VideoApp.Web.Infrastructure.Configuration;

public sealed class MediaOptions
{
    public string RelativePath { get; init; } = "media";
    public string[] AllowedExtensions { get; init; } = [".mp4"];
    public int UploadLimitMB { get; init; } = 200;
}
