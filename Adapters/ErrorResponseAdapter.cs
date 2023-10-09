using proxy_net.Models;
using ServiceReference;

namespace proxy_net.Adapters
{
    public class ErrorResponseAdapter : ResponseError
    {
        public ResponseError @return { get; set; }

        public ErrorResponseAdapter(IErrorResponse response)
        {
            @return = new ResponseError
            {
                code = response.@return.code,
                msg = response.@return.msg,
                error = response.@return.error
            };
        }
    }
}
