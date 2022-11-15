using Microsoft.AspNetCore.Http;

namespace HtmlConverter.Application.Interfaces
{
    public interface IFileStore
    {
        public Task<int> UploadFile(IFormFile file);
        public Task<string> DownloadFile(int id);
    }
}
