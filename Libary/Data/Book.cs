using System;
using System.Collections.Generic;

namespace Libary.Data;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public DateOnly? PublicationYear { get; set; }

    public int NumberBook { get; set; }

    public string PictureBook { get; set; } = null!;

    public int AuthorId { get; set; }

    public int CategoryId { get; set; }

    public int? Price { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<BillInfo> BillInfos { get; set; } = new List<BillInfo>();

    public virtual Category Category { get; set; } = null!;
}
