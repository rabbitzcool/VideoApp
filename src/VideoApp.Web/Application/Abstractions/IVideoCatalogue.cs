using VideoApp.Web.Entities;

namespace VideoApp.Web.Application.Abstractions;

public interface IVideoCatalogue
{
    public Task<IReadOnlyList<VideoItem>> ListAsync(CancellationToken cancelationToken);
}
