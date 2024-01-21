using System;
using System.Collections.Generic;

namespace HappyProgramming_SWP391_GROUP1.Models
{
    public partial class Comment
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public Guid AccountId { get; set; }
        public Guid PostId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;
    }
}
