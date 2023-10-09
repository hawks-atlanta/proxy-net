using Microsoft.AspNetCore.Mvc;
using System.Net;
using proxy_net.Models;

namespace proxy_net.Handler
{
    public static class ErrorResponseHandler
    {
        public static IActionResult HandleResponseError<T>(this ControllerBase controller, T response) where T : class, IResponse
        {
            if (response?.@return == null)
            {
                return controller.NotFound(new ResponseError
                {
                    code = 404,
                    msg = "Not found",
                    error = true
                });
            }

            var statusCode = (HttpStatusCode)response.@return.code;
            if (response.@return.error)
            {
                return controller.StatusCode((int)statusCode, new ResponseError
                {
                    code = response.@return.code,
                    msg = response.@return.msg,
                    error = true
                });
            }

            if (statusCode == HttpStatusCode.NoContent)
            {
                return controller.NoContent();
            }

            return controller.StatusCode((int)statusCode, new ResponseError
            {
                code = response.@return.code,
                msg = response.@return.msg,
                error = false
            });
        }
    }
}
