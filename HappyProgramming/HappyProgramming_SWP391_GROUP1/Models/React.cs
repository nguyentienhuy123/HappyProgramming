using System;
using System.Collections.Generic;

namespace HappyProgramming_SWP391_GROUP1.Models
{
    public partial class React
    {
        public Guid Id { get; set; }
        public bool? Like { get; set; }
        public Guid? AccountId { get; set; }
        public Guid? PostId { get; set; }

        public virtual Account? Account { get; set; }
        public virtual Post? Post { get; set; }
    }
}
