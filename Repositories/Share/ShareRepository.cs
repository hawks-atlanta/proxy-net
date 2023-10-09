using ServiceReference;

namespace proxy_net.Repositories.Share
{
    public class ShareRepository : IShareRepository
    {
        public async Task<share_fileResponse> ShareFile(reqShareFile reqShareFile)
        {
            //var credentials = AdaptersToSoap.ConvertToCredentials(user);
            using (var client = new ServiceClient())
            {
                return await client.share_fileAsync(reqShareFile);
            }
        }

        public async Task<share_listResponse> ShareList(authorization authorization)
        {
            //var credentials = AdaptersToSoap.ConvertToCredentials(user);
            using (var client = new ServiceClient())
            {
                return await client.share_listAsync(authorization);
            }
        }

        public async Task<share_list_with_whoResponse> ShareListWithWho(reqFile reqFile)
        {
            using (var client = new ServiceClient())
            {
                return await client.share_list_with_whoAsync(reqFile);
            }
        }
    }
}
