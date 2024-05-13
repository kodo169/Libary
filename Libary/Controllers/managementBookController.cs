using Microsoft.AspNetCore.Mvc;

namespace Libary.Controllers
{
    public class managementBookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
