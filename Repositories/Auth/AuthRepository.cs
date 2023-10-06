using ServiceReference;

namespace proxy_net.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        public async Task<auth_loginResponse> LoginAsync(credentials credentials)
        {
            //var credentials = AdaptersToSoap.ConvertToCredentials(user);
            using (var client = new ServiceClient())
            {
                return await client.auth_loginAsync(credentials);
            }
        }
        public async Task<auth_refreshResponse> RefreshAsync(authorization authorization)
        {
            //var credentials = AdaptersToSoap.ConvertToCredentials(user);
            using (var client = new ServiceClient())
            {
                return await client.auth_refreshAsync(authorization);
            }
        }
    }
}
