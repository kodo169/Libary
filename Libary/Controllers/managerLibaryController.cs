using Libary.Data;
using Microsoft.AspNetCore.Mvc;

namespace Libary.Controllers
{
    public class managerLibaryController : Controller
    {
        private readonly LibaryContext _data;
        public managerLibaryController(LibaryContext data)
        {
            _data = data;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
