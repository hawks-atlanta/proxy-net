using proxy_net.Models.Auth.Entities;
using ServiceReference;

namespace proxy_net.Adapters
{
    public static class AdaptersToSoap
    {
        public static credentials ConvertToCredentials(User user)
        {
            return new credentials
            {
                username = user.Username,
                password = user.Password
            };
        }
        public static authorization ConvertToAuthorization(string token)
        {
            return new authorization
            {
                token = token
            };
        }
    }
}
