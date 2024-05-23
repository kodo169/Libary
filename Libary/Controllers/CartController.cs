//using Libary.Data;
//using Libary.ViewComponents;
//using Libary.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Text;
//using X.PagedList;

//namespace Libary.Controllers
//{
//    public class CartController : Controller
//    {
//        // Dòng này khai báo một trường riêng tư, 
//        // chỉ đọc có tên _data thuộc loại LibaryContext. 
//        // Điều này sẽ được sử dụng để tương tác với bối cảnh cơ sở dữ liệu.
//        private readonly LibaryContext _data;
//        // Đây là hàm tạo của lớp CartController. 
//        // Nó lấy một đối tượng LibaryContext làm tham số và gán nó vào trường _data.
//        //  Điều này thiết lập nội xạ phụ thuộc cho bối cảnh cơ sở dữ liệu.
//        public CartController(LibaryContext data)
//        {
//            _data = data;
//        }
//        // Phương thức Index được đánh dấu bằng thuộc tính Route, xác định rằng nó sẽ đáp ứng các yêu cầu tại đường dẫn URL /mainIndex.
//        [Route("/mainIndex")]

//        // Phương thức này lấy một trang tham số nguyên có thể rỗng, đại diện cho số trang hiện tại để phân trang.
//        public IActionResult Index(int? page)
//        {
//            // Dòng này đặt số lượng mục được hiển thị trên mỗi trang là 5.
//            int pageSize = 5;
//            // Điều này xác định số trang hiện tại. Nếu trang rỗng hoặc nhỏ hơn 0, pageNumber được đặt thành 1; 
//            // nếu không, nó được đặt thành giá trị của trang.
//            int pageNumber  = page == null || page < 0 ? 1 : page.Value;
//            // Thao tác này sẽ khởi tạo một bộ bills có thể truy vấn được từ ngữ cảnh cơ sở dữ liệu _data.
//            var cart = _data.Bills.AsQueryable();

//            // Dòng này sử dụng phương thức Include của Entity Framework để chỉ định rằng thuộc tính điều hướng BillInfos
//            // phải được đưa vào truy vấn, cho phép tải nhanh dữ liệu liên quan.
//            var result = cart
//                .Include(p => p.BillInfos)
//                // Điều này sử dụng phương thức Select của LINQ để chiếu từng thực thể Book vào một đối tượng cart_ViewModels mới, 
//                // chọn các thuộc tính cụ thể để đưa vào mô hình khung nhìn. 
//                .Select(p => new cart_ViewModels
//                {
//                    idBill = p.idbill,
//                    nameBook = p.Title,
//                    pictureBook = p.PictureBook,
//                    contentBook = p.Content,
//                    nameCategory = p.Category.CategoryName,
//                    idCategory =p.CategoryId,
//                }).AsNoTracking().OrderBy(p => p.nameCategory);
//                // AsNoTracking được sử dụng để cải thiện hiệu suất bằng cách không theo dõi các thực thể trong ngữ cảnh vì dữ liệu ở dạng chỉ đọc. 
//                // Các kết quả được sắp xếp theo thuộc tính nameCategory.

//            // Điều này tạo ra một PagedList mới gồm các đối tượng cart_ViewModels, 
//            // chuyển vào kết quả truy vấn, số trang hiện tại và kích thước trang. Lớp PagedList được giả định để xử lý logic phân trang.
//            PagedList<cart_ViewModels> lst = new PagedList<cart_ViewModels>(result, pageNumber, pageSize);
//            // Điều này trả về lst (danh sách được phân trang của cart_ViewModels) cho chế độ xem sẽ được hiển thị cho người dùng.
//            return View(lst);
//        }
//    }
//}
