using Libary.Data;
using Libary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Libary.ViewComponents
{
    public class PeopleRentalViewComponent : ViewComponent
    {
        public readonly LibaryContext _data;
        public PeopleRentalViewComponent(LibaryContext data) => _data = data;
        public IViewComponentResult Invoke()
        {
            var data = _data.BillInfos
                .Include(x => x.User)
                .Include(x => x.IdbillNavigation)
                .ToList();
            var result = new List<PeopleRental>();
            foreach (var item in data)
            {
                var checkadd = true;
                var dataAdd = new PeopleRental
                {
                    userID = item.UserId,
                    nameUser = item.User.Name,
                    Loan_Date = item.IdbillNavigation.LoanDate,
                    DueDate = item.IdbillNavigation.DueDate,
                    numberBookRental = item.CountBook,
                    Rental_Code = item.Idbill,
                };
                for (int i = 0; i < result.Count; i++)
                {
                    if (item.Idbill == result[i].Rental_Code)
                    {
                        result[i].numberBookRental += item.CountBook ?? 0;
                        checkadd = false;
                        continue;
                    }
                }
                if(checkadd == true) result.Add(dataAdd);
            }
            return View(result);
        }
    }
}
