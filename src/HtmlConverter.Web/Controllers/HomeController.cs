using HtmlConverter.Application.Interfaces;
using HtmlConverter.Domain.Models.enums;
using Microsoft.AspNetCore.Mvc;

namespace HtmlConverter.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFileConverter _htmlToPdfConverter;

        public HomeController(IFileConverter htmlToPdfConverter)
            => _htmlToPdfConverter = htmlToPdfConverter;

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile? file)
        {
            if (file == null)
                throw new FileNotFoundException();

            var taskId = await _htmlToPdfConverter.Convert(file, FileFormat.PDF);

            return RedirectToAction(nameof(Info), new { taskId });
        }

        public IActionResult Info(string taskId)
        {
            ViewBag.TaskId = taskId;

            return View();
        }
    }
}