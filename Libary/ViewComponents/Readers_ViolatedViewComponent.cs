using Libary.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Libary.ViewComponents
{
    public class Readers_ViolatedViewComponent : ViewComponent
    {
        public readonly LibaryContext _data;
        public Readers_ViolatedViewComponent(LibaryContext data) => _data = data;

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
