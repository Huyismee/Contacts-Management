using System;
using System.Collections.Generic;

namespace PRN231_FinalProject.Models
{
    public partial class ContactEmail
    {
        public int ContactEmailId { get; set; }
        public int ContactId { get; set; }
        public string Email { get; set; } = null!;
        public string? Label { get; set; }
    }
}
