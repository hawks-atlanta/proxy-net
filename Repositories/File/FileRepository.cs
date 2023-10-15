using proxy_net.Repositories.File;
using ServiceReference;

namespace proxy_net.Repositories
{
    public class FileRepository : IFileRepository
    {
        public async Task<file_uploadResponse> FileUploadAsync(reqFileUpload reqFileUpload)
        {
            using (var client = new ServiceClient())
            {
                return await client.file_uploadAsync(reqFileUpload);
            }
        }

        public async Task<file_checkResponse> FileCheckAsync(reqFile reqFile)
        {
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

        public async Task<file_listResponse> FileListAsync(reqFileList reqFileList)
        {
            using (var client = new ServiceClient())
            {
                return await client.file_listAsync(reqFileList);
            }
        }

        public async Task<file_deleteResponse> FileDeleteAsync(reqFileDelete reqFileDelete)
        {
            using (var client = new ServiceClient())
            {
                return await client.file_deleteAsync(reqFileDelete);
            }
        }

        public async Task<file_renameResponse> FileRenameAsync(reqFileRename reqFileRename)
        {
            using (var client = new ServiceClient())
            {
                return await client.file_renameAsync(reqFileRename);
            }
        }
    }
}