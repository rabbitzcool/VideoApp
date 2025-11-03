namespace VideoApp.Web.Application.Abstractions;

public interface IVideoStorage
{
    public Task SaveAsync(IFormFile file, CancellationToken cancellationToken);
}
