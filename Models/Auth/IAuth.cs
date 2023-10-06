namespace proxy_net.Models.Auth
{
    public interface IAuthLogin
    {
        string? Username { get; set; }
        string? Password { get; set; }
    }
    
    public interface IAuthRefresh
    {
        string? Token { get; set; }
    }
}