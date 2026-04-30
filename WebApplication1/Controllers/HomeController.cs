using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : Controller
    {
       
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
           
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Templates", "Home");

            string htmlContent = await System.IO.File.ReadAllTextAsync(Path.Combine(folderPath, "index.html"));
            string cssContent = await System.IO.File.ReadAllTextAsync(Path.Combine(folderPath, "styles.css"));

            string finalHtml = htmlContent.Replace("{{STYLES}}", cssContent);

           
            return Content(finalHtml, "text/html", System.Text.Encoding.UTF8);
        }
    }
}
