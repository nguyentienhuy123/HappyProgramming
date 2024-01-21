using System;
using System.Collections.Generic;

namespace HappyProgramming_SWP391_GROUP1.Models
{
    public partial class Request
    {
        public Guid Id { get; set; }
        public string? Status { get; set; }
        public Guid SendId { get; set; }
        public Guid ReceiveId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid CourseId { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual Account Receive { get; set; } = null!;
        public virtual Account Send { get; set; } = null!;
    }
}
