namespace Libary.ViewModels
{
    public class Reader_Violated
    {
        public int userID {  get; set; }
        public string name { get; set; }
        public DateOnly DueDate { get; set; }
        
        public int? numberBookHire { get; set; }
        public string idBill { get; set; }
    }
}
