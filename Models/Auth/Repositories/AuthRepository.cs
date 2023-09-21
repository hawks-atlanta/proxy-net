using proxy_net.Controllers.SimulatedServices;
using proxy_net.Models.Auth.Entities;

namespace proxy_net.Models.Auth.Repositories
{
    public interface IAuthRepository
    {
        Task<SoapResponse> PostLoginAsync(string username, string password);
        Task<SoapResponse> PostRegisterAsync(string username, string password);
        Task<SoapResponse> PostChallengeAsync(string jwt);
    }
}
