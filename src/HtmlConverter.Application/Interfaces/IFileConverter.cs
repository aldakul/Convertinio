using HtmlConverter.Domain.Models;
using HtmlConverter.Domain.Models.enums;
using Microsoft.AspNetCore.Http;

namespace HtmlConverter.Application.Interfaces
{
    public interface IFileConverter
    {
        Task<string> Convert(IFormFile? file, FileFormat fileFormat);
        Task<OutputFile> GetResult(string jobId);
    }
}
