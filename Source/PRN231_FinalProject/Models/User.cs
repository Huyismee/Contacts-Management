using System;
using System.Collections.Generic;

namespace PRN231_FinalProject.Models
{
    public partial class User
    {
        public User()
        {
            Contacts = new HashSet<Contact>();
            Labels = new HashSet<Label>();
        }

        public int UserId { get; set; }
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string FullName { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<Label> Labels { get; set; }
    }
}
