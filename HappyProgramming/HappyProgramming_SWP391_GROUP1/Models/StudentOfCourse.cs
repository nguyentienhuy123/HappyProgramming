using System;
using System.Collections.Generic;

namespace HappyProgramming_SWP391_GROUP1.Models
{
    public partial class StudentOfCourse
    {
        public long Id { get; set; }
        public Guid AccountId { get; set; }
        public Guid CourseId { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Course Course { get; set; } = null!;
    }
}
