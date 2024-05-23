using Libary.Data;
using Microsoft.AspNetCore.Mvc;

namespace Libary.Controllers
{
    public class addCartController : Controller
    {
        private readonly LibaryContext _data;
        public addCartController(LibaryContext data)
        {
            _data = data;
        }
        public IActionResult Index() 
        {
            return View();
        }
    }
}
