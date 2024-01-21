using System.ComponentModel.DataAnnotations;

namespace HappyProgramming_SWP391_GROUP1.Models.ViewModel
{
    public class CourseViewModel
    {
        public string Email { get; set; }
        public DateTime? StartDate { get; set; }      
        public DateTime? EndDate { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
