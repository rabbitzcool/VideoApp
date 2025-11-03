using MediatR;
using VideoApp.Web.Shared;

namespace VideoApp.Web.Application.Videos.Upload;

public sealed record UploadVideosCommand(IFormFileCollection Files) : IRequest<ApiResult>;