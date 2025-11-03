using MediatR;
using VideoApp.Web.Application.Abstractions;
using VideoApp.Web.Entities;
using VideoApp.Web.Shared;

namespace VideoApp.Web.Application.Videos.List;

public sealed class GetCatalogueQueryHandler(IVideoCatalogue catalogue)
        : IRequestHandler<GetCatalogueQuery, ApiResult<IReadOnlyList<VideoItem>>>
{
    private readonly IVideoCatalogue catalogue = catalogue;

    public Task<ApiResult<IReadOnlyList<VideoItem>>> Handle(GetCatalogueQuery request, CancellationToken cancellationToken)
        => this.catalogue
        .ListAsync(cancellationToken)
        .ContinueWith(t => ApiResult<IReadOnlyList<VideoItem>>
            .Success(t.Result),
            cancellationToken);
}