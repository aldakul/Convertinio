using HtmlConverter.Application.Common.Infrastructure;
using HtmlConverter.Application.Common.Utils;
using HtmlConverter.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HtmlConverter.Application.FileConverter
{
    public class FileStore : IFileStore
    {
        private readonly IBaseRepository<Domain.Models.File> _fileRepository;

        public FileStore(IBaseRepository<Domain.Models.File> fileRepository) =>
            _fileRepository = fileRepository;

        public async Task<string> DownloadFile(int id)
        {
            var file = await _fileRepository.Files.FirstOrDefaultAsync(x => x.Id == id);
            if (file == null)
                throw new FileNotFoundException();
            var fileWithDirectory = ConfigHelper.GetDownloadFileWithDirectory(file);

            ConfigHelper.InitializeFolder(fileWithDirectory);

            await using FileStream writer = File.Create(fileWithDirectory);
            await writer.WriteAsync(file.FileData);

            return fileWithDirectory;
        }

        public async Task<int> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new FileNotFoundException();

            var fileName = Path.GetFileName(file.FileName);
            var fileExtension = Path.GetExtension(fileName);
            var fileFormat = FileExtensions.GetFileFormat(fileExtension);
            using var fileData = new MemoryStream();
            await file.CopyToAsync(fileData);

            var inputFile = new Domain.Models.File
            {
                Name = fileName,
                FileFormat = fileFormat,
                FileData = fileData.ToArray(),
                Created = DateTime.Now
            };

            await _fileRepository.Files.AddAsync(inputFile);
            await _fileRepository.SaveChangesAsync();
            return inputFile.Id;
        }
    }
}
