using Microsoft.AspNetCore.Mvc;

namespace Post.Aggregate.Service.Controller;

public class HomeController : ControllerBase
{
    // GET
    public IActionResult Index()
    {
        return Redirect("~/swagger");
    }
}