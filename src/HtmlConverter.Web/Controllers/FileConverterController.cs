using HtmlConverter.Application.Common.Exceptions;
using HtmlConverter.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HtmlConverter.Web.Controllers
{
    public class FileConverterController : Controller
    {
        private readonly IFileConverter _htmlToPdfConverter;
        private readonly IConverterJobStatus _converterTaskStatus;
        public FileConverterController(IFileConverter htmlToPdfConverter, IConverterJobStatus converterTaskStatus)
            => (_htmlToPdfConverter, _converterTaskStatus) = (htmlToPdfConverter, converterTaskStatus);

        [HttpGet]
        public async Task<IActionResult> Download(string taskId)
        {
            var taskStatusId = _converterTaskStatus.GetJobStatus(taskId);
            if (taskStatusId == null)
                throw new NotFoundException(taskId);

            var outputFile = await _htmlToPdfConverter.GetResult(taskId);
            
            if (outputFile == null)
                throw new FileNotFoundException();

            if (outputFile.FileDirectory == null)
                throw new NullReferenceException();

            if (outputFile.ContentType == null)
                throw new NullReferenceException();

            return PhysicalFile(outputFile.FileDirectory, outputFile.ContentType, outputFile.FileName);
        }
    }
}
