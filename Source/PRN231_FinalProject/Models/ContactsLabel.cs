using System;
using System.Collections.Generic;

namespace PRN231_FinalProject.Models
{
    public partial class ContactsLabel
    {
        public int ContactLabelId { get; set; }
        public int LabelId { get; set; }
        public int ContactId { get; set; }

        public virtual Contact? Contact { get; set; } = null!;
        public virtual Label? Label { get; set; } = null!;
    }
}
