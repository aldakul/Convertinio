using File = HtmlConverter.Domain.Models.File;
using Hangfire;
using HtmlConverter.Application.Common.Utils;
using HtmlConverter.Application.FileConverter.Pdf;
using HtmlConverter.Application.Interfaces;
using HtmlConverter.Domain.Models;
using HtmlConverter.Domain.Models.enums;
using Microsoft.AspNetCore.Http;
using HtmlConverter.Application.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HtmlConverter.Application.FileConverter
{
    public class FileConverter : IFileConverter
    {
        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IBaseRepository<File> _fileRepository;
        private readonly IFileStore _fileStore;
        public FileConverter(IBackgroundJobClient backgroundJobClient, IBaseRepository<File> fileRepository, IFileStore fileStore) =>
            (_backgroundJobClient, _fileRepository, _fileStore) = (backgroundJobClient, fileRepository, fileStore);

        public async Task<string> Convert(IFormFile? file, FileFormat fileFormat)
        {
            if (file == null)
                throw new FileNotFoundException();

            var fromFileFormatName = Path.GetFileName(file.FileName);
            var fromFileFormatExtension = Path.GetExtension(fromFileFormatName);

            var fromFileFormat = FileExtensions.GetFileFormat(fromFileFormatExtension);
            
            if (!(fromFileFormat == FileFormat.HTML &&
                fileFormat == FileFormat.PDF))
                throw new FormatException();

            var id = await _fileStore.UploadFile(file);

            var result = _backgroundJobClient.Enqueue<PdfConvertJob>(x => x.Convert(id));

            return result;
        }

        public async Task<OutputFile> GetResult(string jobId)
        {
            var monitoringApi = JobStorage.Current.GetMonitoringApi();
            var jobDetailsDto = monitoringApi.JobDetails(jobId);

            if (jobDetailsDto == null)
                throw new NotFoundException(jobId);

            var stateHistoryDto = jobDetailsDto.History.SingleOrDefault(x => x.StateName == "Succeeded");

            if (stateHistoryDto == null)
                throw new NullReferenceException();

            var jobResultId = stateHistoryDto.Data["Result"];
            var fileId = int.Parse(jobResultId);


            var downloadFile = await _fileStore.DownloadFile(fileId);

            var file = await _fileRepository.Files.SingleAsync(x => x.Id == fileId);

            var outputFile = new OutputFile
            {
                ContentType = file.ContentType!,
                FileName = file.Name,
                FileDirectory = downloadFile
            };
            return outputFile;
        }
    }
}
