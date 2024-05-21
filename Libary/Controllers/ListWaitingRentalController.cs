using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Libary.Controllers
{
    public class ListWaitingRentalController : Controller
    {
        private readonly LibaryContext _dataSQLServer;
        public ListWaitingRentalController(LibaryContext dataSQLServer)
        {
            _dataSQLServer = dataSQLServer;
        }

        [HttpGet]
        public IActionResult Index(string queryUserName = "")
        {
            // Xây dựng truy vấn cơ bản bao gồm các điều kiện cố định
            var query = _dataSQLServer.BillInfos
                .Include(bi => bi.IdbillNavigation)
                .Include(b => b.User)
                .Include(bi => bi.Book)
                .Where(bi => (!bi.IdbillNavigation.StandbyStatus.HasValue || bi.IdbillNavigation.StandbyStatus == false)
                             && (!bi.IdbillNavigation.StatusDone.HasValue || bi.IdbillNavigation.StatusDone == false));

            // Áp dụng bộ lọc tìm kiếm nếu có
            if (!string.IsNullOrEmpty(queryUserName))
            {
                query = query.Where(bi => bi.User.Name.Contains(queryUserName));
            }

            var data = query.ToList();
            var result = new List<ListWaitingRental_ViewModels>();

            // Duyệt qua danh sách kết quả truy vấn
            foreach (var item in data)
            {
                var checkadd = true;

                var addresult = new ListWaitingRental_ViewModels
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

        [HttpPost]
        public async Task<IActionResult> CheckBill(string rentalCode)
        {
            try
            {
                var bill = await _dataSQLServer.Bills.FirstOrDefaultAsync(b => b.Idbill == rentalCode);
                if (bill != null)
                {
                    bill.StandbyStatus = true;
                    await _dataSQLServer.SaveChangesAsync();
                }
                else
                {
                    TempData["ErrorMessage"] = "Bill not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to update the bill status: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> XBill(string rentalCode)
        {
            try
            {
                var bill = await _dataSQLServer.Bills.FirstOrDefaultAsync(b => b.Idbill == rentalCode);
                if (bill != null)
                {
                    bill.StandbyStatus = false;
                    bill.StatusDone = true;
                    await _dataSQLServer.SaveChangesAsync();
                }
                else
                {
                    TempData["ErrorMessage"] = "Bill not found.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to update the bill status: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
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
    }
}
