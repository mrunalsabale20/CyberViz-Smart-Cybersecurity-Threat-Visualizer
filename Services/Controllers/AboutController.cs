using Microsoft.AspNetCore.Mvc;

namespace SmartCyberViz.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index() => View();
    }
}