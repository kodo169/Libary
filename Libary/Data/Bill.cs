using System;
using System.Collections.Generic;

namespace Libary.Data;

public partial class Bill
{
    public string Idbill { get; set; }

    public DateOnly LoanDate { get; set; }

    public DateOnly DueDate { get; set; }

    public DateOnly? ReturnDate { get; set; }

    public bool? StandbyStatus { get; set; }

    public bool? StatusDone { get; set; }

    public virtual ICollection<BillInfo> BillInfos { get; set; } = new List<BillInfo>();
}
