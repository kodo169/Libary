using Libary.Data;
using Microsoft.AspNetCore.Mvc;

namespace Libary.Controllers
{
    public class EditBookController : Controller
    {
        private readonly LibaryContext _data;
        public EditBookController(LibaryContext data)
        {
            _data = data;
        }

        public IActionResult addBook()
        {
            return View();
        }
    }
}
