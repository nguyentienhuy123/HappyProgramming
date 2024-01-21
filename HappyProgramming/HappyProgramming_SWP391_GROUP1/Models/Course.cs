using System;
using System.Collections.Generic;

namespace HappyProgramming_SWP391_GROUP1.Models
{
    public partial class Course
    {
        public Course()
        {
            Requests = new HashSet<Request>();
            StudentOfCourses = new HashSet<StudentOfCourse>();
        }

        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<StudentOfCourse> StudentOfCourses { get; set; }
    }
}
