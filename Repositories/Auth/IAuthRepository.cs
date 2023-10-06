using ServiceReference;

namespace proxy_net.Repositories
{
    public interface IAuthRepository
    {
        Task<auth_loginResponse> LoginAsync(credentials credentials);
        Task<auth_refreshResponse> RefreshAsync(authorization authorization);
    }
}
