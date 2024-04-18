using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Libary.ViewComponents
{
    public class RelatedViewComponent : ViewComponent
    {
        public readonly LibaryContext _data;
        public RelatedViewComponent(LibaryContext data) => _data = data;
        public IViewComponentResult Invoke(string nameAuthor)
        {
            var data = _data.Books
                .Include(p => p.Author)
                .Include(p => p.Category)
                .Where(p => p.Author.Name == nameAuthor)
                .Select(dt => new relatedProduct_ViewModels
                {
                    picturebook = dt.PictureBook,
                    nameBook = dt.Title,
                    idBook = dt.BookId,
                    nameCategory = dt.Category.CategoryName,
                });
            return View(data);
        }
    }
}
