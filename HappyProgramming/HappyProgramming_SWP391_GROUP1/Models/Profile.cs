using System;
using System.Collections.Generic;

namespace HappyProgramming_SWP391_GROUP1.Models
{
    public partial class Profile
    {
        public Profile()
        {
            Accounts = new HashSet<Account>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Dob { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Avatar { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
