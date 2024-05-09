using Microsoft.AspNetCore.Mvc;

namespace Management.Photo.Service.Controller
{
    public class HomeController : ControllerBase
    {
        // GET
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}