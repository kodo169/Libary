using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Libary.ViewComponents
{
    public class menuCategoryViewComponent : ViewComponent
    {
        public readonly LibaryContext _data;
        public menuCategoryViewComponent(LibaryContext data) => _data = data;

        public IViewComponentResult Invoke(int? idcategory)
        {
            var data = _data.Categories.Select(dataCategory => new menuCategory_ViewModels
            {
                categoryID=dataCategory.CategoryId, categoryName= dataCategory.CategoryName,
            });
            return View(data);
        }
    }
}
