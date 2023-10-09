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
    }
}
