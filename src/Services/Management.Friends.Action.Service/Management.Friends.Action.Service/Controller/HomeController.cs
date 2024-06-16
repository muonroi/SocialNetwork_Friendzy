namespace Matched.Friend.Service.Controller;

public class HomeController : ControllerBase
{
    // GET
    public IActionResult Index()
    {
        return Redirect("~/swagger");
    }
}