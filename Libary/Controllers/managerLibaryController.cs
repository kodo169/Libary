using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            if (Global.role == false)
            {
                return Redirect("/mainIndex");
            }
            return View();
        } 
    }
}
