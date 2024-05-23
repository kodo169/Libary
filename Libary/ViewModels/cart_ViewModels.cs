namespace Libary.ViewModels
{
    public class cart_ViewModels
    {
        public int idbill  { get; set; }
        public int userID { get; set; }
        public int? numberBookRental { get; set; }
        public int Rental_Code { get; set; }
        public int? idBook { get; set; }
        public string? nameBook { get; set; }
        public string? nameUser { get; set; }
        public DateOnly Loan_Date { get; set; }
        public DateOnly DueDate { get; set; }
    }
}
