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
        [Route("/mainIndex")]
        public IActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNumber  = page == null || page < 0 ? 1 : page.Value;
            var listBook = _data.Books.AsQueryable();

            var result = listBook
                .Include(p => p.Category)
                .Select(p => new listBook_ViewModels
                {
                    idBook = p.BookId,
                    nameBook = p.Title,
                    pictureBook = p.PictureBook,
                    contentBook = p.Content,
                    nameCategory = p.Category.CategoryName,
                    idCategory =p.CategoryId,
                }).AsNoTracking().OrderBy(p => p.nameCategory);
            PagedList<listBook_ViewModels> lst = new PagedList<listBook_ViewModels>(result, pageNumber, pageSize);
            return View(lst);
        }
        public IActionResult categoryBook(int? category, int? page)
        {
            int pageSize = 6;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var listBook = _data.Books.AsQueryable();

            if (category.HasValue)
            {
                listBook = listBook.Where(p => p.CategoryId == category.Value);
            }
            var result = listBook
                .Include(p => p.Category)
                .Select(p => new listBook_ViewModels
                {
                    idBook = p.BookId,
                    nameBook = p.Title,
                    pictureBook = p.PictureBook,
                    contentBook = p.Content,
                    nameCategory = p.Category.CategoryName,
                    idCategory = p.CategoryId,
                }).AsNoTracking().OrderBy(p => p.nameCategory);
            PagedList<listBook_ViewModels> lst = new PagedList<listBook_ViewModels>(result, pageNumber, pageSize);
            return View(lst);
        }
        public IActionResult searchs(string? query, int? page)
        {
            int pageSize = 6;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var listbook = _data.Books.Include(p => p.Author).AsQueryable();
            if (query != null)
            {
                listbook = listbook.Where(p => p.Title.Contains(query) || p.Author.Name.Contains(query));
            }
            var result = listbook.Select(p => new listBook_ViewModels
            {
                idBook = p.BookId,
                nameBook = p.Title,
                pictureBook = p.PictureBook,
                contentBook = p.Content,
                querySearch = query,
            }).AsNoTracking();
            PagedList<listBook_ViewModels> lst = new PagedList<listBook_ViewModels>(result, pageNumber, pageSize);
            return View(lst);
        }
        public IActionResult Detail(int? id)
        {
            var data = _data.Books
                .Include(p => p.Category)
                .Include(p => p.Author)
                .SingleOrDefault(p => p.BookId == id);
            if (data == null)
            {
                TempData["Message"] = "We’re sorry, the page you have looked for does not exist in our website! Maybe go to our home page or try to use a search?";
                return Redirect("/404");
            }
            var result = new informationProducts_ViewModels
            {
                idBook = data.BookId,
                nameBook = data.Title,
                pictureBook = data.PictureBook,
                contentBook = data.Content,
                numberBook = data.NumberBook,
                PublicationYearBook = data.PublicationYear,
                nameAuthor = data.Author.Name,
                nameCategory = data.Category.CategoryName
            };
            return View(result);
        }
    }
}
