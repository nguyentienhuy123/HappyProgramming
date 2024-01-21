using System;
using System.Collections.Generic;

namespace HappyProgramming_SWP391_GROUP1.Models
{
    public partial class Notification
    {
        public Guid Id { get; set; }
        public Guid? Content { get; set; }
        public Guid AccountId { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Href { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
