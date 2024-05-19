namespace Libary.ViewModels
{
    public class newImportedBook
    {

        public int BookId { get; set; }
        public int? Price { get; set; }
        public int? amount { get; set; }
        public DateOnly? DateCreate { get; set; }
        public string Title { get; set; } = null!;
    }
}
