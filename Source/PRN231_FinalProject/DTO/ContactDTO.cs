﻿using PRN231_FinalProject.Models;

namespace PRN231_FinalProject.DTO;

public class ContactDTO
{ 
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
    public virtual ICollection<ContactEmail> ContactEmails { get; set; }
    public virtual ICollection<ContactPhone> ContactPhones { get; set; }
    public virtual ICollection<ContactsLabelDTO> ContactsLabels { get; set; }

}

public class ContactsLabelDTO
{
    public int ContactLabelId { get; set; }
    public int LabelId { get; set; }
    public int ContactId { get; set; }
}