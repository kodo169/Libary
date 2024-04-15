using System;
using System.Collections.Generic;

namespace Libary.Data;

public partial class Staff
{
    public int StaffId { get; set; }

    public string Name { get; set; } = null!;

    public string Position { get; set; } = null!;

    public string? ContactInfo { get; set; }
}
