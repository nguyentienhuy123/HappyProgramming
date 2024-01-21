namespace HappyProgramming_SWP391_GROUP1.Models.ViewModel
{
    public class RequestViewModel
    {   
        public Guid Id { get; set; }
        public string? Status { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? CourseName { get; set; }
    }
}
