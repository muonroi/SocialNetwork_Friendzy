namespace User.Service.Controllers;

public class HomeController : ControllerBase
{
    // GET
    public IActionResult Index()
    {
        return Redirect("~/swagger");
    }
}