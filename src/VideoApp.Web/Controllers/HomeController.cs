using Microsoft.AspNetCore.Mvc;

namespace VideoApp.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => this.View();
}
