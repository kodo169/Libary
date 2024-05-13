using System;
using System.Collections.Generic;
using Libary.Data;

namespace Libary;

public partial class RolePermission
{
    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    public int RolePermissionId { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
