using System;
using System.Collections.Generic;

namespace HappyProgramming_SWP391_GROUP1.Models
{
    public partial class MessageDetail
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public Guid? SendId { get; set; }
        public Guid? ReceiveId { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? IsClass { get; set; }
        public bool? IsRead { get; set; }

        public virtual Account? Receive { get; set; }
        public virtual Account? Send { get; set; }
    }
}
