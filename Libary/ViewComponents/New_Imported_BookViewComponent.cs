using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Libary.ViewComponents
{
    public class New_Imported_BookViewComponent : ViewComponent
    {
        private readonly LibaryContext _data;
        public New_Imported_BookViewComponent(LibaryContext data)
        {
            _data = data;
        }
        public IViewComponentResult Invoke()
        {
            DateOnly datecheck = DateOnly.FromDateTime(DateTime.Now);
            var dataBook = _data.Books.ToList();
            var dataBillInfor = _data.BillInfos
                                .Include(x => x.User)
                                .ToList();
            var result = new List<newImportedBook>();
            foreach (var item in dataBook)
            {
                if (item.DateCreate == null || item.DateCreate.Value.Month - datecheck.Month > 1) continue;
                var datacheck = new newImportedBook
                {
                    DateCreate = item.DateCreate,
                    BookId = item.BookId,
                    Title = item.Title,
                    Price = item.Price,
                    amount = item.NumberBook,
                };
                result.Add(datacheck);
            }
            return View(result);
        }
        
    }
}
