using Microsoft.AspNetCore.Mvc;

namespace SysPescaderiaSaavedra.Web.Controllers
{
    public class Dashboard : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
