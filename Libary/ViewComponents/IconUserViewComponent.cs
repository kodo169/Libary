using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Libary.ViewComponents
{
    public class IconUserViewComponent : ViewComponent
    {
        private readonly LibaryContext _data;
        public IconUserViewComponent(LibaryContext data)
        {
            _data = data;
        }
        public IViewComponentResult Invoke()
        {
            var data = _data.Users.Where(p => p.UserId == Global.id_User);
            var result = data.Select(p => new DataUser_ViewModels
            {
                id = p.UserId,
                nameAcc = p.Username,
                email = p.Email,
                role = p.Role,
                name = p.Name,
            }).ToList();
            return View(result);
        }
    }
}
