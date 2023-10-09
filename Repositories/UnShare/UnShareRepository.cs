﻿using ServiceReference;

namespace proxy_net.Repositories.UnShare
{
    public class UnShareRepository : IUnShareRepository
    {
        public async Task<unshare_fileResponse> UnShareFile(reqShareRemove reqShareRemove)
        {
            //var credentials = AdaptersToSoap.ConvertToCredentials(user);
            using (var client = new ServiceClient())
            {
                return await client.unshare_fileAsync(reqShareRemove);
            }
        }
    }
}
