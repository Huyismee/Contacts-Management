using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN231_FinalProject.Models
{
    public partial class Contact
    {
        public Contact()
        {
            ContactEmails = new HashSet<ContactEmail>();
            ContactPhones = new HashSet<ContactPhone>();
            ContactsLabels = new HashSet<ContactsLabel>();
        }

        public int ContactId { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Notes { get; set; }
        public DateTime? History { get; set; }
        public bool? Favorite { get; set; }
        public string? ProfileImage { get; set; }
        public int UserId { get; set; }
        public DateTime? DeleteDate { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ContactEmail> ContactEmails { get; set; }
        public virtual ICollection<ContactPhone> ContactPhones { get; set; }
        public virtual ICollection<ContactsLabel> ContactsLabels { get; set; }
    }
}
