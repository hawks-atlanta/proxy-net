using proxy_net.Models.Auth.DataSources;
using proxy_net.Models.Auth.Entities;

namespace proxy_net.Infrastructure.DataSources
{
    public class AuthDatasourcesImpl : IAuthDatasource
    {
        /// <summary>
        /// Método que implementa la llamada SOAP simulada.
        /// </summary>
        /// <param name="username">Username enviado desde el body</param>
        /// <param name="password">Password enviado desde el body</param>
        /// <returns>Respuesta simulada del SOAP con un JWT quemado</returns>
        public Task<SoapResponse> PostLoginAsync(string username, string password)
        {
            // Simula la llamada SOAP y devuelve una respuesta
            // TODO: Implementar la llamada SOAP REAL y devolver la respuesta
            if (username == "usuario" && password == "contraseña")
            {
                return Task.FromResult(new SoapResponse { Jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c3VhcmlvX3BydWViYSIsImp0aSI6IjEwMDEyMyIsImlhdCI6MTYzMTg5MjA5NH0.kT3PN4Ue6nGLPQ8gXaJ3Am4LwkbWx-6RuvVLFO0wUNw" });
            }
            else
            {
                return Task.FromResult(new SoapResponse { ErrorMessage = "Autenticación fallida" });
            }
        }

        public Task<SoapResponse> PostRegisterAsync(string username, string password)
        {
            // Simula la llamada SOAP y devuelve una respuesta
            // TODO: Implementar la llamada SOAP REAL y devolver la respuesta
            if (username == "usuario" && password == "contraseña")
            {
                return Task.FromResult(new SoapResponse { Jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c3VhcmlvX3BydWViYSIsImp0aSI6IjEwMDEyMyIsImlhdCI6MTYzMTg5MjA5NH0.kT3PN4Ue6nGLPQ8gXaJ3Am4LwkbWx-6RuvVLFO0wUNw" });
            }
            else
            {
                return Task.FromResult(new SoapResponse { ErrorMessage = "Autenticación fallida" });
            }
        }

        public Task<SoapResponse> PostChallengeAsync(string jwt)
        {
            // Simula la llamada SOAP y devuelve una respuesta
            if (jwt == "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c3VhcmlvX3BydWViYSIsImp0aSI6IjEwMDEyMyIsImlhdCI6MTYzMTg5MjA5NH0.kT3PN4Ue6nGLPQ8gXaJ3Am4LwkbWx-6RuvVLFO0wUNw")
            {
                return Task.FromResult(new SoapResponse { Jwt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJ1c3VhcmlvX3BydWViYSIsImp0aSI6IjEwMDEyMyIsImlhdCI6MTYzMTg5MjA5NH0.kT3PN4Ue6nGLPQ8gXaJ3Am4LwkbWx-6RuvVLFO0wUNw" });
            }
            else
            {
                return Task.FromResult(new SoapResponse { ErrorMessage = "Autenticación fallida" });
            }
        }
    }
}
