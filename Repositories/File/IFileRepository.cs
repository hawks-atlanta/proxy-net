using ServiceReference;

namespace proxy_net.Repositories.File
{
    public interface IFileRepository
    {
        Task<file_uploadResponse> FileUploadAsync(reqFileUpload reqFileUpload);
        Task<file_checkResponse> FileCheckAsync(reqFile reqFileCheck);
    }
}
