using HtmlConverter.Application.Common.Infrastructure;
using HtmlConverter.Application.Common.Utils;
using HtmlConverter.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using PuppeteerSharp;

namespace HtmlConverter.Application.FileConverter.Pdf
{
    public class PdfConvertJob
    {
        private const string _fileFormat = "pdf";
        private readonly IBaseRepository<Domain.Models.File> _fileRepository;
        public PdfConvertJob(IBaseRepository<Domain.Models.File> baseRepository) 
            => _fileRepository = baseRepository;

        public async Task<int> Convert(int id)
        {
            var file = await _fileRepository.Files.SingleAsync(x => x.Id == id);
            var uploadFileWithDirectory = ConfigHelper.GetUploadFileWithDirectory(file);

            if (file.FileData == null)
                throw new NullReferenceException();

            ConfigHelper.InitializeFolder(uploadFileWithDirectory);
            await File.WriteAllBytesAsync(uploadFileWithDirectory, file.FileData);

            var downloadFileWithDirectory = ConfigHelper.GetDownloadFileWithDirectory(file, _fileFormat);
            ConfigHelper.InitializeFolder(downloadFileWithDirectory);
            
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            await using var page = await browser.NewPageAsync();
            await page.GoToAsync(uploadFileWithDirectory);
            await page.PdfAsync(downloadFileWithDirectory);

            await page.CloseAsync();
            await browser.CloseAsync();

            if (!File.Exists(downloadFileWithDirectory))
            {
                throw new FileNotFoundException();
            }
            
            var fileFormat = FileExtensions.GetFileFormat(_fileFormat);
            var fileData = await File.ReadAllBytesAsync(downloadFileWithDirectory);


            var downloadFile = new Domain.Models.File
            {
                ContentType = ConfigHelper.GetContentType(),
                Name = $"{file.Name}.{_fileFormat}",
                FileData = fileData,
                FileFormat = fileFormat,
                Created = DateTime.UtcNow
            };

            await _fileRepository.Files.AddAsync(downloadFile);
            await _fileRepository.SaveChangesAsync();


            File.Delete(uploadFileWithDirectory);
            File.Delete(downloadFileWithDirectory);

            return downloadFile.Id;
        }
    }
}
