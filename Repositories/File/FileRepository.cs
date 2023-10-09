﻿using proxy_net.Repositories.File;
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

        public async Task<file_downloadResponse> FileDownloadAsync(reqFile reqFile)
        {
            using (var client = new ServiceClient())
            {
                return await client.file_downloadAsync(reqFile);
            }
        }

        public async Task<file_new_dirResponse> FileNewDirAsync(reqFileNewDir reqFileNewDir)
        {
            using (var client = new ServiceClient())
            {
                return await client.file_new_dirAsync(reqFileNewDir);
            }
        }

        public async Task<file_moveResponse> FileMoveAsync(reqFileMove reqFileMove)
        {
            using (var client = new ServiceClient())
            {
                return await client.file_moveAsync(reqFileMove);
            }
        }
    }
}