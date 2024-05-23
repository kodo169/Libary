using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Libary.Controllers
{
    public class ReaderViolatedController : Controller
    {
        private readonly LibaryContext _dataSQLServer;
        public ReaderViolatedController(LibaryContext dataSQLServer)
        {
            _dataSQLServer = dataSQLServer;
        }

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
            var result = new List<ReaderViolatedViewModel>();

            // Duyệt qua danh sách kết quả truy vấn
            foreach (var item in data)
            {
                // Chuyển đổi DateOnly thành DateTime để so sánh
                var dueDateTime = item.IdbillNavigation.DueDate.ToDateTime(TimeOnly.MinValue);

                // Chỉ thêm vào danh sách nếu DueDate nhỏ hơn ngày hiện tại
                if (dueDateTime < DateTime.Now && !item.IdbillNavigation.ReturnDate.HasValue)
                {
                    var checkadd = true;

                    var addresult = new ReaderViolatedViewModel
                    {
                        userID = item.UserId,
                        name = item.User.Name,
                        DueDate = item.IdbillNavigation.DueDate.ToDateTime(TimeOnly.MinValue),
                        numberBookHire = item.CountBook,
                        idBill = item.Idbill,
                        StandbyStatus = item.IdbillNavigation.StandbyStatus,
                        StatusDone = item.IdbillNavigation.StatusDone,
                        Email = item.User.Email,
                        ReturnDate = item.IdbillNavigation.ReturnDate
                    };

                    // Kiểm tra xem kết quả đã tồn tại trong danh sách chưa
                    for (int i = 0; i < result.Count; i++)
                    {
                        if (item.Idbill == result[i].idBill)
                        {
                            result[i].numberBookHire += item.CountBook ?? 0;
                            checkadd = false;
                            break;
                        }
                    }

                    // Nếu chưa có trong danh sách, thêm mới
                    if (checkadd) result.Add(addresult);
                }
            }

            return View(result);
        }

        // Các phương thức khác
        [HttpPost]
        public async Task<IActionResult> CheckBill(string rentalCode)
        {
            try
            {
                var bill = await _dataSQLServer.Bills.FirstOrDefaultAsync(b => b.Idbill == rentalCode);
                if (bill != null)
                {
                    bill.StatusDone = true;
                    bill.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
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
