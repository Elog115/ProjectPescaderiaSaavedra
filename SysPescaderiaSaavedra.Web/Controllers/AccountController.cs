using Microsoft.AspNetCore.Mvc;

namespace SysPescaderiaSaavedra.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
