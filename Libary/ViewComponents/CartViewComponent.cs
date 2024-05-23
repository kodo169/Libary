using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Libary.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly LibaryContext _data;
        public CartViewComponent(LibaryContext data)
        {
            _data = data;
        }
        public IViewComponentResult Invoke()
        {
            var data = _data.BillInfos
                .Include(p => p.IdbillNavigation)
                .Where(p => p.IdbillNavigation.StatusDone == false && p.UserId == Global.id_User && p.IdbillNavigation.StandbyStatus == null)
                .ToList();
            Global.hireBook = data.Count;
            return View();
        }
    }
}
