using MediatR;
using VideoApp.Web.Entities;
using VideoApp.Web.Shared;

namespace VideoApp.Web.Application.Videos.List;

public sealed record GetCatalogueQuery() : IRequest<ApiResult<IReadOnlyList<VideoItem>>>;