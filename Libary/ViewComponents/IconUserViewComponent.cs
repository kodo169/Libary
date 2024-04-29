using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Libary.ViewComponents
{
    public class IconUserViewComponent : ViewComponent
    {
        private readonly LibaryContext _context;
        public IconUserViewComponent(LibaryContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
