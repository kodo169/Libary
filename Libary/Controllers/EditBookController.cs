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
            //bool checkCategory = false;
            //bool checkAuthor = false;
            var newBook = new Book();
            if (_addBook != null && _addBook.amount != 0)
            {
                if(_addBook.Odercategoty != "0")
                {
                    var datacheckCategory = _data.Categories.Where(p=>p.CategoryName == _addBook.Odercategoty).FirstOrDefault();
                    newBook.CategoryId = datacheckCategory.CategoryId;
                }
                else
                {
                    var addnewCategory = new Category();
                    addnewCategory.CategoryName = _addBook.categoty;
                    _data.Add(addnewCategory);
                    _data.SaveChanges();
                    var datacheckCategory = _data.Categories.Where(p => p.CategoryName == _addBook.categoty).FirstOrDefault();
                    newBook.CategoryId = datacheckCategory.CategoryId;

                }
                if (_addBook.Oderauthor != "0")
                {
                    var datacheckAuthor = _data.Authors.Where(p => p.Name == _addBook.Oderauthor).FirstOrDefault();
                    newBook.AuthorId = datacheckAuthor.AuthorId;
                }
                else
                {
                    var addnewAuthor = new Author();
                    addnewAuthor.Name = _addBook.categoty;
                    _data.Add(addnewAuthor);
                    _data.SaveChanges();
                    var datacheckAuthor = _data.Authors.Where(p => p.Name == _addBook.author).FirstOrDefault();
                    newBook.CategoryId = datacheckAuthor.AuthorId;

                }
                newBook.PictureBook = Global.namePictute;
                newBook.Title = _addBook.Title;
                newBook.NumberBook = _addBook.amount;
                newBook.Price = _addBook.price;
                newBook.PublicationYear = _addBook.PublicationYear;

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
