using proxy_net.Controllers.SimulatedServices;
using proxy_net.Models.Auth.DataSources;
using proxy_net.Models.Auth.Entities;
using proxy_net.Models.Auth.Repositories;

namespace proxy_net.Infrastructure.Repositories
{
    public class AuthRepositoryImpl : IAuthRepository
    {
        private readonly IAuthDatasource _authDatasource;

        public AuthRepositoryImpl(IAuthDatasource authDatasource)
        {
            _authDatasource = authDatasource;
        }

        public Task<SoapResponse> PostLoginAsync(string username, string password)
        {
            return _authDatasource.PostLoginAsync(username, password);
        }

        public Task<SoapResponse> PostRegisterAsync(string username, string password)
        {
            return _authDatasource.PostRegisterAsync(username, password);
        }

        public Task<SoapResponse> PostChallengeAsync(string jwt)
        {
            return _authDatasource.PostChallengeAsync(jwt);
        }
    }
}
