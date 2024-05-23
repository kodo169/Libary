using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace Libary.Controllers
{
    public class ListReaderBorrowingController : Controller
    {
        private readonly LibaryContext _dataSQLServer;
        public ListReaderBorrowingController(LibaryContext dataSQLServer)
        {
            _dataSQLServer = dataSQLServer;
        }
        public IActionResult Index()
        {
            var data = _dataSQLServer.BillInfos.Include(bi => bi.IdbillNavigation).
                Include(b => b.User)
                .Include(bi => bi.Book).
                Where(bi => (!bi.IdbillNavigation.StandbyStatus.HasValue || bi.IdbillNavigation.StandbyStatus == true)
                                          && (!bi.IdbillNavigation.StatusDone.HasValue || bi.IdbillNavigation.StatusDone == false))
                .ToList();   
            var result = new List<ListReaderBorrowing_ViewModels>();
            foreach (var item in data)
            {
                var checkadd = true;

                var addresult = new ListReaderBorrowing_ViewModels
                {
                    idBook = item.BookId,
                    nameBook = item.Book.Title,
                    UserID = item.UserId,
                    Email = item.User.Email,
                    LoanDate = item.IdbillNavigation.LoanDate,
                    ReturnDate = item.IdbillNavigation.ReturnDate,
                    countBook = item.CountBook,
                    UserName = item.User.Name,
                    RentalCode = item.Idbill,
                    StandbyStatus = item.IdbillNavigation.StandbyStatus,
                    StatusDone = item.IdbillNavigation.StatusDone,
                    price = item.Book.Price
                };
                for (int i = 0; i < result.Count; i++)
                {
                    if (item.Idbill == result[i].RentalCode)
                    {
                        result[i].countBook += item.CountBook ?? 0;
                        checkadd = false;
                        continue;
                    }
                }
                if (checkadd == true) result.Add(addresult);
            }
            return View(result);
        }
        [HttpPost]
        public IActionResult CheckRental(string billId)
        {
            var billInfo = _dataSQLServer.BillInfos
                .Include(bi => bi.IdbillNavigation)
                .FirstOrDefault(bi => bi.Idbill == billId);

            if (billInfo != null)
            {
                // Cập nhật thuộc tính StandbyStatus và StatusDone
                billInfo.IdbillNavigation.StandbyStatus = false;
                billInfo.IdbillNavigation.StatusDone = true;
                billInfo.IdbillNavigation.ReturnDate = DateOnly.FromDateTime(DateTime.Now);

                _dataSQLServer.SaveChanges();
            }

            // Chuyển hướng đến action Index để reload dữ liệu
            return RedirectToAction("Index");
        }

        public IActionResult GetBookDetails(string billId)
        {
            var bookInfos = _dataSQLServer.BillInfos
                .Include(bi => bi.Book)
                .Include(bi => bi.Book.Author) // Giả sử rằng mỗi cuốn sách có liên kết với tác giả
                .Where(bi => bi.Idbill == billId)
                .Select(bi => new
                {
                    title = bi.Book.Title,
                    author = bi.Book.Author.Name,
                    quantity = bi.CountBook
                })
                .ToList();

            if (bookInfos == null || bookInfos.Count == 0)
            {
                return NotFound();
            }

            return Json(bookInfos);
        }


        [HttpGet]
        public IActionResult Index(string queryUserName = "")
        {
            // Xây dựng truy vấn cơ bản bao gồm các điều kiện cố định
            var query = _dataSQLServer.BillInfos
                .Include(bi => bi.IdbillNavigation)
                .Include(b => b.User)
                .Include(bi => bi.Book)
                .Where(bi => (!bi.IdbillNavigation.StandbyStatus.HasValue || bi.IdbillNavigation.StandbyStatus == true)
                             && (!bi.IdbillNavigation.StatusDone.HasValue || bi.IdbillNavigation.StatusDone == false));

            // Áp dụng bộ lọc tìm kiếm nếu có
            if (!string.IsNullOrEmpty(queryUserName))
            {
                query = query.Where(bi => bi.User.Name.Contains(queryUserName));
            }

            var data = query.ToList();
            var result = new List<ListReaderBorrowing_ViewModels>();

            // Duyệt qua danh sách kết quả truy vấn
            foreach (var item in data)
            {
                var checkadd = true;

                var addresult = new ListReaderBorrowing_ViewModels
                {
                    idBook = item.BookId,
                    nameBook = item.Book.Title,
                    UserID = item.UserId,
                    Email = item.User.Email,
                    LoanDate = item.IdbillNavigation.LoanDate,
                    ReturnDate = item.IdbillNavigation.ReturnDate,
                    countBook = item.CountBook,
                    UserName = item.User.Name,
                    RentalCode = item.Idbill,
                    StandbyStatus = item.IdbillNavigation.StandbyStatus,
                    StatusDone = item.IdbillNavigation.StatusDone,
                    price = item.Book.Price
                };

                // Kiểm tra xem kết quả đã tồn tại trong danh sách chưa
                for (int i = 0; i < result.Count; i++)
                {
                    if (item.Idbill == result[i].RentalCode)
                    {
                        result[i].countBook += item.CountBook ?? 0;
                        checkadd = false;
                        break;
                    }
                }

                // Nếu chưa có trong danh sách, thêm mới
                if (checkadd) result.Add(addresult);
            }

            return View(result);
        }

        // Action Index để hiển thị danh sách chờ xác nhận mượn sách
    }
}
