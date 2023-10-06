
using ServiceReference;

namespace proxy_net.Repositories.Account
{
    public class AccountRepository : IAccountRepository
    {
        public async Task<account_registerResponse> AccountRegisterAsync(credentials credentials)
        {
            //var credentials = AdaptersToSoap.ConvertToCredentials(user);
            using (var client = new ServiceClient())
            {
                return await client.account_registerAsync(credentials);
            }
        }
        public async Task<account_passwordResponse> AccountPasswordAsync(reqAccPassword reqAccPassword)
        {
            //var credentials = AdaptersToSoap.ConvertToCredentials(user);
            using (var client = new ServiceClient())
            {
                return await client.account_passwordAsync(reqAccPassword);
            }
        }
    }
}
