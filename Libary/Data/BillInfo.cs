using System;
using System.Collections.Generic;

namespace Libary.Data;

public partial class BillInfo
{
    public int IdbillInfo { get; set; }

    public int BookId { get; set; }

    public int UserId { get; set; }

    public int Idbill { get; set; }

    public int? CountBook { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual Bill IdbillNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
