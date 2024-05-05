using Microsoft.AspNetCore.Mvc;

namespace SearchPartners.Aggregate.Service.Controller;

public class HomeController : ControllerBase
{
    // GET
    public IActionResult Index()
    {
        return Redirect("~/swagger");
    }
}