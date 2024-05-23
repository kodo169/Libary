using Libary.Data;
using Libary.ViewComponents;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using X.PagedList;

namespace Libary.Controllers
{
    public class CartController : Controller
    {
        private readonly LibaryContext _data;
        public CartController(LibaryContext data)
        {
            _data = data;
        }
        [Route("/addCart")]
        public IActionResult Index()
        {
            var data = _data.BillInfos
                .Include(p => p.IdbillNavigation)
                .Where(p => p.IdbillNavigation.StatusDone == false && p.UserId == Global.id_User && p.IdbillNavigation.StandbyStatus == null)
                .ToList();
            var result = new List<cart_ViewModels>();
            foreach (var item in data)
            {
                var inforBook = _data.Books.Where(p =>p.BookId == item.BookId).FirstOrDefault();
                var addResult = new cart_ViewModels
                {
                    idbill = item.Idbill,
                    userID = item.UserId,
                    numberBookRental = item.CountBook,
                    idBook = item.BookId,
                    nameBook = inforBook?.Title,
                    pictureBook = inforBook?.PictureBook,
                    money = inforBook?.Price ?? 0,
                    idbillinfor = item.IdbillInfo,
                };
                result.Add(addResult);
            }
            return View(result);
        }
        public IActionResult plus_minusBook(int numberBook, int idBillInfor)
        {
            var data = _data.BillInfos.Where(p => p.IdbillInfo == idBillInfor).FirstOrDefault();
            if(data == null) return Redirect("/404");
            data.CountBook = numberBook;
            _data.SaveChanges();
            return Redirect("/addCart");
        }
        public IActionResult removeAddCart(int idBillInfor)
        {
            var data = _data.BillInfos.Where(p => p.IdbillInfo == idBillInfor).FirstOrDefault();
            var dataRemove = new BillInfo();
            dataRemove = data;
            if(dataRemove != null) _data.Remove(dataRemove);
            _data.SaveChanges();
            return Redirect("/addCart");
        }
        public IActionResult actionOder(string idBill)
        {
            var data = _data.Bills.Where(p => p.Idbill == idBill).FirstOrDefault();
            if (data == null) return Redirect("/404");
            data.StandbyStatus = false;
            _data.SaveChanges();
            Global.hireBook = 0;
            return Redirect("/addCart");
        }
        public IActionResult addOrderCart()
        {
            return Redirect("/mainIndex");
        }
    }
}
