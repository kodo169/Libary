using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Libary.ViewComponents
{
    public class role_ManagerViewComponent : ViewComponent
    {
        public readonly LibaryContext _data;
        public role_ManagerViewComponent(LibaryContext data) => _data = data;
        public IViewComponentResult Invoke()
        {
            var data = _data.RolePermissions.Where(p => p.RoleId == Libary.ViewModels.Global.id_User).ToList();
            var result = new List<roleManagement>();
            foreach (var item in data)
            {
                var addresult = new roleManagement
                {
                    checkRole = item.PermissionId,
                };
                result.Add(addresult);
            }
            return View(result);
        }
    }
}
