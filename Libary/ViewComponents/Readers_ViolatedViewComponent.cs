using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Libary.ViewComponents
{
    public class Readers_ViolatedViewComponent : ViewComponent
    {
        public readonly LibaryContext _data;
        public Readers_ViolatedViewComponent(LibaryContext data) => _data = data;

        public IViewComponentResult Invoke()
        {
            DateOnly checkDay = DateOnly.FromDateTime(DateTime.Now);
            var data = _data.BillInfos
                .Include(p => p.User)
                .Include(p => p.IdbillNavigation);
            var result = new List<Reader_Violated>();
            foreach (var item in data)
            {
                bool check = true;
                var addResult = new Reader_Violated
                {
                    userID = item.UserId,
                    name = item.User.Name,
                    DueDate = item.IdbillNavigation.DueDate,
                    numberBookHire = item.CountBook,
                    idBill = item.Idbill,
                };
                if (item.IdbillNavigation.DueDate.Year <= checkDay.Year || item.IdbillNavigation.DueDate.Month <= checkDay.Month || item.IdbillNavigation.DueDate.Day <= checkDay.Day)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        if (item.Idbill == result[i].idBill)
                        {
                            result[i].numberBookHire += item.CountBook ?? 0;
                            check = false;
                            continue;
                        }
                    }
                    if(check == true) result.Add(addResult);
                }
            }
            return View(result);
        }
    }
}
