using ServiceReference;

namespace proxy_net.Repositories.Share
{
    public interface IShareRepository
    {
        Task<share_fileResponse> ShareFile(reqShareFile reqShareFile);
    }
}
