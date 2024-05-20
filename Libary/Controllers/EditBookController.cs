using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
