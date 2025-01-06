using System;
using System.Collections.Generic;

namespace PRN231_FinalProject.Models
{
    public partial class ContactPhone
    {
        public int ContactPhoneId { get; set; }
        public string Phone { get; set; } = null!;
        public int ContactId { get; set; }
        public string Code { get; set; } = null!;
        public string? Label { get; set; }

    }
}
