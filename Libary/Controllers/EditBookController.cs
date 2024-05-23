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
