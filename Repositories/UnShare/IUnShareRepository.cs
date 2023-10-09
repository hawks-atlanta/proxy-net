using ServiceReference;

namespace proxy_net.Repositories.UnShare
{
    public interface IUnShareRepository
    {
        Task<unshare_fileResponse> UnShareFile(reqShareRemove reqShareRemove);
    }
}
