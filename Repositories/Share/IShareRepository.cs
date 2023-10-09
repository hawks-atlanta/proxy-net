using ServiceReference;

namespace proxy_net.Repositories.Share
{
    public interface IShareRepository
    {
        Task<share_fileResponse> ShareFile(reqShareFile reqShareFile);
        Task<share_listResponse> ShareList(authorization authorization);
        Task<share_list_with_whoResponse> ShareListWithWho(reqFile reqFile);
    }
}
