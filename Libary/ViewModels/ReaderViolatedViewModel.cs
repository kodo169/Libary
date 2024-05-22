namespace Libary.ViewModels
{
    public class ReaderViolatedViewModel
    {
        public int? userID { get; set; }
        public string? name { get; set; }
        public DateTime? DueDate { get; set; }

        public DateOnly? ReturnDate { get; set; }

        public int? numberBookHire { get; set; }
        public string? idBill { get; set; }

        public bool? StandbyStatus { get; set; }

        public bool? StatusDone { get; set; }

        public string? Email { get; set; }
    }
}
