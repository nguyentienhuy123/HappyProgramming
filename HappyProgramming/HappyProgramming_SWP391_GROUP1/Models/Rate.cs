using System;
using System.Collections.Generic;

namespace HappyProgramming_SWP391_GROUP1.Models
{
    public partial class Rate
    {
        public Guid Id { get; set; }
        public int? Rate1 { get; set; }
        public Guid AccountId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Account Account { get; set; } = null!;
    }
}
