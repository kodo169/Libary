namespace Libary.ViewModels
{
    public class addBook
    {
        public string? Title { get; set; }
        public int amount { get; set; }
        public string? Odercategoty { get; set; }
        public string? categoty { get; set; }
        public int? price { get; set; }
        public string? author { get; set; }
        public string? Oderauthor { get; set; }
        public DateOnly? PublicationYear { get; set; }

        public string? Description { get; set; }
        public string? namePicture { get; set; }
    }
}
