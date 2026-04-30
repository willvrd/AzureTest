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
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Layouts", "index.html");
            string htmlContent = await System.IO.File.ReadAllTextAsync(filePath);

            return Content(htmlContent, "text/html", System.Text.Encoding.UTF8);
        }
    }
}
