using proxy_net.Models.Auth.DataSources;
using proxy_net.Models.Auth.Entities;
using System.Threading.Tasks;

namespace proxy_net.Controllers.SimulatedServices
{
    public class MockSoapService : IAuthDatasource
    {
        public Task<SoapResponse> PostLoginAsync(string username, string password)
        {
            // Simula la llamada SOAP y devuelve una respuesta
            var response = new SoapResponse
            {
                // Rellena los campos según lo que SOAP devolvería
            };
            return Task.FromResult(response);
        }

        public Task<SoapResponse> PostRegisterAsync(string username, string password)
        {
            // Simula la llamada SOAP y devuelve una respuesta
            var response = new SoapResponse
            {
                // Rellena los campos según lo que SOAP devolvería
            };
            return Task.FromResult(response);
        }

        public Task<SoapResponse> PostChallengeAsync(string jwt)
        {
            // Simula la llamada SOAP y devuelve una respuesta
            var response = new SoapResponse
            {
                // Rellena los campos según lo que SOAP devolvería
            };
            return Task.FromResult(response);
        }
    }
}
