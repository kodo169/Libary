namespace Libary.ViewModels
{
    public class PeopleRental
    {
        public int userID { get; set; }
        public string? nameUser { get; set; }

        public DateOnly Loan_Date { get; set; }
        public DateOnly DueDate { get; set; }
        public int? numberBookRental { get; set; }
        public string Rental_Code { get; set; }
    }
}
