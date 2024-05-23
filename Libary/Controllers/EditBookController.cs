using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Libary.Controllers
{
    public class EditBookController : Controller
    {
        private readonly LibaryContext _data;
        public EditBookController(LibaryContext data)
        {
            _data = data;
        }
        public IActionResult Cancel() 
        {
            Global.namePictute = "";
            return Redirect("/inforBook");
        }
        [HttpPost]
        public IActionResult actionAddBook(addBook _addBook)
        {
            var newBook = new Book();
            if (_addBook != null && _addBook.amount != 0)
            {
                newBook.AuthorId = 1;
                newBook.PictureBook = Global.namePictute;
                newBook.Title = _addBook.Title;
                newBook.NumberBook = _addBook.amount;
                newBook.Price = _addBook.price;
                newBook.PublicationYear = _addBook.PublicationYear;
                newBook.CategoryId = 1;
                newBook.DateCreate = DateOnly.FromDateTime(DateTime.Now);
                newBook.Content = _addBook.Description;
                _data.Books.Add(newBook);
                _data.SaveChanges();
            }
            Global.namePictute = "";
            return Redirect("/inforBook");
        }
        [Route("/addBook")]
        public IActionResult addBook(addBook addBook)
        {
            return View(addBook);
        }
        [HttpGet]
        [Route("/EditBook/EditBook/{id}")]
        public IActionResult EditBook(int id)
        {
            var book = _data.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefault(b => b.BookId == id);

            if (book == null)
            {
                return NotFound();
            }

            var model = new addBook
            {
                idBook = book.BookId,
                Title = book.Title,
                amount = book.NumberBook,
                price = book.Price,
                author = book.Author.Name,
                categoty = book.Category.CategoryName,
                PublicationYear = book.PublicationYear,
                Description = book.Content
            };

            return View(model);
        }

        [HttpPost]
        [Route("/EditBook/EditBook/{id}")]
        public IActionResult EditBook(int id, addBook updatedBook)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedBook);
            }

            var book = _data.Books.FirstOrDefault(b => b.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            book.Title = updatedBook.Title;
            book.NumberBook = updatedBook.amount;
            book.Price = updatedBook.price;
            book.Content = updatedBook.Description;
            book.PublicationYear = updatedBook.PublicationYear;
            book.AuthorId = _data.Authors.FirstOrDefault(a => a.Name == updatedBook.author)?.AuthorId ?? book.AuthorId;
            book.CategoryId = _data.Categories.FirstOrDefault(c => c.CategoryName == updatedBook.categoty)?.CategoryId ?? book.CategoryId;

            _data.SaveChanges();

            return Redirect("/inforBook");
        }

        [HttpPost]
        [Route("EditBook/DeleteBook/{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = _data.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }

            _data.Books.Remove(book);
            _data.SaveChanges();

            return Redirect("/inforBook"); // Redirect to the index view or wherever you list the books
        }

        [HttpPost]
        public async Task<IActionResult> addPictureBook(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                Global.namePictute = file.FileName;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            return Redirect("/addBook");
        }

    }
}
