using Libary.Data;
using Microsoft.AspNetCore.Mvc;

namespace Libary.ViewComponents
{
    public class authorViewComponent : ViewComponent
    {
        private readonly LibaryContext _data;
        public authorViewComponent(LibaryContext data)
        {
            _data = data;
        }
        public IViewComponentResult Invoke()
        {
            var data = _data.Authors.ToList();
            return View(data);
        }
    }
}
