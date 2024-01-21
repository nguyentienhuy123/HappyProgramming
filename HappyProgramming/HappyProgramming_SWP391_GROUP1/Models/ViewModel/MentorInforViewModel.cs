namespace HappyProgramming_SWP391_GROUP1.Models.ViewModel
{
    public class MentorInforViewModel
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid? PostId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Name { get; set; } = null!;
        public Profile MentorProfile { get; set; }
    }
}
