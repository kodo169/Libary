using Libary.Data;
using Libary.ViewComponents;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using X.PagedList;

namespace Libary.Controllers
{
    public class listBookController : Controller
    {
        private readonly LibaryContext _data;
        public listBookController(LibaryContext data)
        {
            _data = data;
        }
        public IActionResult Index(int? category)
        {
            var listBook = _data.Books.AsQueryable();

            if (category.HasValue)
            {
                listBook = listBook.Where(p => p.CategoryId == category.Value);
            }
            var result = listBook.Select(p => new listBook_ViewModels
            {
                idBook = p.BookId,
                nameBook = p.Title,
                pictureBook = p.PictureBook,
                contentBook = p.Content
            }).ToList();
            return View(result);
        }

        public IActionResult Search(string? query)
        {
            var listbook = _data.Books.AsQueryable();
            if (query != null)
            {
                listbook = listbook.Where(p => p.Title.Contains(query));
            }
            var result = listbook.Select(p => new listBook_ViewModels
            {
                idBook = p.BookId,
                nameBook = p.Title,
                pictureBook = p.PictureBook,
                contentBook = p.Content,
            }).ToList();
            return View(result);
        }
    }
}
