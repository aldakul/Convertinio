using HtmlConverter.Application.Common.Exceptions;
using HtmlConverter.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HtmlConverter.Web.Controllers
{
    public class ConverterTaskStatusController : Controller
    {
        private readonly IConverterJobStatus  _converterTaskStatus;
        public ConverterTaskStatusController(IConverterJobStatus converterTaskStatus) 
            => _converterTaskStatus = converterTaskStatus;
        [HttpGet]
        public IActionResult GetTaskStatus(string taskId)
        {
            var taskStatusId = _converterTaskStatus.GetJobStatus(taskId);
            if(taskStatusId == null)
                throw new NotFoundException(taskId);
            return Ok(taskStatusId);
        }
    }
}
