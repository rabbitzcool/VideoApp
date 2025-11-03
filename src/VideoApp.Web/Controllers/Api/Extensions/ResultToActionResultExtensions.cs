using Microsoft.AspNetCore.Mvc;
using VideoApp.Web.Shared;

namespace VideoApp.Web.Controllers.Api.Extensions;

public static class ResultToActionResultExtensions
{
    public static IActionResult ToActionResult(this ApiResult apiResult, ControllerBase c)
    {
        if (apiResult.IsSuccess)
        {
            return c.Ok();
        }

        return c.Problem(apiResult);
    }

    public static IActionResult ToActionResult<T>(this ApiResult<T> apiResult, ControllerBase c)
    {
        if (apiResult.IsSuccess)
        {
            return c.Ok(apiResult.Value);
        }

        return c.Problem(apiResult);
    }

    private static ObjectResult Problem(this ControllerBase c, ApiResult apiResult)
        => Problem(c, apiResult.Error);

    private static ObjectResult Problem<T>(this ControllerBase c, ApiResult<T> apiResult)
        => Problem(c, apiResult.Error);

    private static ObjectResult Problem(this ControllerBase c, Error error)
    {
        int status = error.Status ?? StatusCodes.Status400BadRequest;
        var pd = new ProblemDetails
        {
            Status = status,
            Title = error.Code,
            Detail = error.Message
        };

        if (error.Details is not null)
        {
            pd.Extensions["details"] = error.Details;
        }

        return new ObjectResult(pd) { StatusCode = status };
    }
}
