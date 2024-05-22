namespace Libary.ViewModels
{
    public class ListWaitingRental_ViewModels
    {
        public int? idBook { get; set; }
        public string? nameBook { get; set; }
        public int? countBook { get; set; }
        public int? UserID { get; set; }
        public DateOnly? LoanDate { get; set; }
        public DateOnly? DueDate { get; set; }
        public bool? StandbyStatus { get; set; }
        public bool? StatusDone { get; set; }
        public int? price { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public string? RentalCode { get; set; }

        public string? author { get; set; }
    }
}
