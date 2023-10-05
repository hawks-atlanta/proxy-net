using proxy_net.Adapters;
using proxy_net.Models.Auth.Entities;
using ServiceReference;
using System;
using System.Threading.Tasks;

namespace proxy_net.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        public async Task<auth_loginResponse> LoginAsync(User user)
        {
            var credentials = AdaptersToSoap.ConvertToCredentials(user);
            using (var client = new ServiceClient())
            {
                return await client.auth_loginAsync(credentials);
            }
        }
        public async Task<account_registerResponse> RegisterAsync(User user)
        {
            var credentials = AdaptersToSoap.ConvertToCredentials(user);
            using (var client = new ServiceClient())
            {
                return await client.account_registerAsync(credentials);
            }
        }
    }
}
