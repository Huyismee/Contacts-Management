using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PRN231_FinalProject.Models
{
    public partial class Label
    {
        public Label()
        {
            ContactsLabels = new HashSet<ContactsLabel>();
        }

        public int LabelId { get; set; }
        public string LabelName { get; set; } = null!;
        public int UserId { get; set; }
        [JsonIgnore]
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ContactsLabel> ContactsLabels { get; set; }
    }
}
