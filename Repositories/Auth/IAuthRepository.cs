using proxy_net.Models.Auth.Entities;
using ServiceReference;
using System.Threading.Tasks;

namespace proxy_net.Repositories
{
    public interface IAuthRepository
    {
        Task<auth_loginResponse> LoginAsync(User user);
        Task<account_registerResponse> RegisterAsync(User user);
    }
}
