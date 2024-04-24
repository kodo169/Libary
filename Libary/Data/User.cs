using System;
using System.Collections.Generic;

namespace Libary.Data;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool Role { get; set; }

    public virtual ICollection<BillInfo> BillInfos { get; set; } = new List<BillInfo>();
}
