using Microsoft.Extensions.Configuration;
using System.IO;

namespace HtmlConverter.Application.Common.Infrastructure
{
    public static class ConfigHelper
    {
        private static readonly IConfigurationRoot Configuration =
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        
        public static void InitializeFolder(string folder)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(folder)!);
        }

        public static string GetDownloadFileWithDirectory(Domain.Models.File file, string fileFormat = "")
        {
            var fileExtension = string.IsNullOrEmpty(fileFormat) ? 
                file.FileFormat.ToString().ToLower() : 
                fileFormat;
            var fileFolder = GetDownloadFileFolder();
            var result = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                fileFolder,
                $"{file.Id}.{fileExtension}");
            
            return result;
        }

        public static string GetUploadFileWithDirectory(Domain.Models.File file)
        {
            var fileFolder = GetUploadFileFolder();
            var result = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                fileFolder,
                $"{file.Id}.{file.FileFormat.ToString().ToLower()}");

            return result;
        }

        private static string GetUploadFileFolder()
            => Configuration["UploadFileFolder"];
        

        public static string GetDownloadFileFolder() 
            => Configuration["DownloadFileFolder"];
        

        public static string GetContentType() 
            => "application/pdf";
    }
}
