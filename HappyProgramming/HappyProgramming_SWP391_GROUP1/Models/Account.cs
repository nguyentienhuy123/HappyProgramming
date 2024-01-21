using System;
using System.Collections.Generic;

namespace HappyProgramming_SWP391_GROUP1.Models
{
    public partial class Account
    {
        public Account()
        {
            Comments = new HashSet<Comment>();
            MessageDetailReceives = new HashSet<MessageDetail>();
            MessageDetailSends = new HashSet<MessageDetail>();
            Posts = new HashSet<Post>();
            Rates = new HashSet<Rate>();
            Reacts = new HashSet<React>();
            RequestReceives = new HashSet<Request>();
            RequestSends = new HashSet<Request>();
            StudentOfCourses = new HashSet<StudentOfCourse>();
        }

        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Role { get; set; }
        public string? Status { get; set; }
        public Guid? ProfileId { get; set; }
        public string Email { get; set; } = null!;
        public string? Pin { get; set; }

        public virtual Profile? Profile { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<MessageDetail> MessageDetailReceives { get; set; }
        public virtual ICollection<MessageDetail> MessageDetailSends { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
        public virtual ICollection<React> Reacts { get; set; }
        public virtual ICollection<Request> RequestReceives { get; set; }
        public virtual ICollection<Request> RequestSends { get; set; }
        public virtual ICollection<StudentOfCourse> StudentOfCourses { get; set; }
    }
}
