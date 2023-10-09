using proxy_net.Models;
using ServiceReference;

namespace proxy_net.Adapters
{
    public class ResponseAdapter : IResponse
    {
        private readonly ResponseError _return;

        public ResponseAdapter(Func<ResponseError> returnFunc)
        {
            _return = returnFunc();
        }

        public ResponseError @return
        {
            get => _return;
            set { /* ... */ }
        }
    }
}
