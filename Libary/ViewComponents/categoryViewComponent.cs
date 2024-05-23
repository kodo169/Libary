using Libary.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Libary.ViewComponents
{
    public class categoryViewComponent : ViewComponent
    {
        private readonly LibaryContext _data;
        public categoryViewComponent(LibaryContext data)
        {
            _data = data;
        }
        public IViewComponentResult Invoke()
        {
            var data = _data.Categories.ToList();
            return View(data);
        }
    }
}
