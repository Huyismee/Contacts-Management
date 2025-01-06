using System;
using System.Collections.Generic;

namespace WebApp
{
    public partial class ContactsLabel
    {
        public int ContactLabelId { get; set; }
        public int LabelId { get; set; }
        public int ContactId { get; set; }
        public virtual Label? Label { get; set; } = null!;

    }
}
