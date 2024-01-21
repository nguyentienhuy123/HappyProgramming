using System;
using System.Collections.Generic;

namespace HappyProgramming_SWP391_GROUP1.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Reacts = new HashSet<React>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public Guid AccountId { get; set; }
        public DateTime CreateDate { get; set; }
        public string? AttachmentPath { get; set; }
        public Guid? CourseId { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<React> Reacts { get; set; }
    }
}
