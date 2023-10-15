using ServiceReference;

namespace proxy_net.Repositories.File
{
    public interface IFileRepository
    {
        Task<file_uploadResponse> FileUploadAsync(reqFileUpload reqFileUpload);
        Task<file_checkResponse> FileCheckAsync(reqFile reqFileCheck);
        Task<file_downloadResponse> FileDownloadAsync(reqFile reqFileDownload);
        Task<file_new_dirResponse> FileNewDirAsync(reqFileNewDir reqFileNewDir);
        Task<file_moveResponse> FileMoveAsync(reqFileMove reqFileMove);
        Task<file_listResponse> FileListAsync(reqFileList reqFileList);
        Task<file_deleteResponse> FileDeleteAsync(reqFileDelete reqFileDelete);
        Task<file_renameResponse> FileRenameAsync(reqFileRename reqFileRename);
    }
}
