using Libary.Data;
using Libary.ViewComponents;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using X.PagedList;

namespace Libary.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibaryContext _data;
        public HomeController(LibaryContext data)
        {
            _data = data;
        }

        public IActionResult Index(int? page)
        {
            return Redirect("/mainIndex");
        }
        [Route("/404")]
        public IActionResult notFound()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
