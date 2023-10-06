using ServiceReference;

namespace proxy_net.Repositories
{
    public interface IAccountRepository
    {
        Task<account_registerResponse> AccountRegisterAsync(credentials credentials);
        Task<account_passwordResponse> AccountPasswordAsync(reqAccPassword reqAccPassword);
    }
}
