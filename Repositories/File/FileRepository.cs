using proxy_net.Repositories.File;
using ServiceReference;

namespace proxy_net.Repositories
{
    public class FileRepository : IFileRepository
    {
        public async Task<file_uploadResponse> FileUploadAsync(reqFileUpload reqFileUpload)
        {
            //var credentials = AdaptersToSoap.ConvertToCredentials(user);
            using (var client = new ServiceClient())
            {
                return await client.file_uploadAsync(reqFileUpload);
            }
        }

        public async Task<file_checkResponse> FileCheckAsync(reqFile reqFile)
        {
            //var credentials = AdaptersToSoap.ConvertToCredentials(user);
            using (var client = new ServiceClient())
            {
                return await client.file_checkAsync(reqFile);
            }

        }
    }
}
