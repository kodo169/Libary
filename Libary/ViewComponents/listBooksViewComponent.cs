using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Libary.ViewComponents
{
    public class listBooksViewComponent : ViewComponent
    {
        private readonly LibaryContext _context;
        public listBooksViewComponent (LibaryContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var data = _context.Books
                .Select(book => new listBook_ViewModels
                {
                    idBook = book.BookId,
                    nameBook = book.Title,
                    pictureBook = book.PictureBook,
                    contentBook = book.Content,
                    numberBook = book.NumberBook
                });
            return View(data);
        }
    }
}
