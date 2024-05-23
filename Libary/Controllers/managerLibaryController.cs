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
            else
            {
                var data = _data.BillInfos.Include(bi => bi.IdbillNavigation).
                Include(b => b.User)
                .Include(bi => bi.Book)
                .ToList();
                var dataDashboard = new List<managerLibrary_ViewModels>();
                var result = new List<managerLibrary_ViewModels>();
                var booksInLibrary = _data.Books.Sum(b => b.NumberBook);
                var viewModel = new managerLibrary_ViewModels
                {
                    totalBooks = _data.Books.Sum(b => b.NumberBook),
                    totalReaders = _data.Users.Count(),
                    BooksBorrowed = _data.BillInfos
                            .Where(bi => (!bi.IdbillNavigation.StandbyStatus.HasValue || bi.IdbillNavigation.StandbyStatus == true)
                                          && (!bi.IdbillNavigation.StatusDone.HasValue || bi.IdbillNavigation.StatusDone == false))
                            .Sum(bi => bi.CountBook)
                };
                result.Add(viewModel);
                return View(result);
            }
            
        }
    }
}
