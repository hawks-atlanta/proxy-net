namespace proxy_net.Models.Auth.Entities
{
    public class User
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? JwtToken { get; set; }
    }
}