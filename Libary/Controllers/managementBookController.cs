using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Libary.Controllers
{
    public class managementBookController : Controller
    {
        private readonly LibaryContext _data;
        public managementBookController(LibaryContext data)
        {
            _data = data;
        }
        public IActionResult Index()
        {
            var data = _data.Books
            .Include(p => p.Category)
            .Include(p => p.Author)
            .ToList();
            var result = new List<informationProducts_ViewModels>();
            foreach (var item in data) 
            {
                var addresult = new informationProducts_ViewModels
                {
                    idBook = item.BookId,
                    nameBook = item.Title,
                    numberBook = item.NumberBook,
                    PublicationYearBook = item.PublicationYear,
                    nameAuthor = item.Author.Name,
                    nameCategory = item.Category.CategoryName,
                    price = item.Price,
                };
                result.Add(addresult);
            }
            return View(result);
        }
    }
}
