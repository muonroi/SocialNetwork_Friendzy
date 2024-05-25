using Microsoft.AspNetCore.Mvc;

namespace Account.Service.Controller
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
