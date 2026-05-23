using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Modules.Core.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public HomeController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
           
            string folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Modules",
                "Core",
                "Templates",
                "Home"
            );

            string htmlContent = await System.IO.File.ReadAllTextAsync(Path.Combine(folderPath, "index.html"));
            string cssContent = await System.IO.File.ReadAllTextAsync(Path.Combine(folderPath, "styles.css"));

            string finalHtml = htmlContent.Replace("{{STYLES}}", cssContent);

            // Set Environment
            finalHtml = finalHtml.Replace("STAGING", _env.EnvironmentName.ToUpper());

            return Content(finalHtml, "text/html", System.Text.Encoding.UTF8);
        }
    }
}