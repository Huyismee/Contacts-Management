using Microsoft.EntityFrameworkCore;
using PRN231_Assignment2_eBookStoreAPI.Repositories.Common;
using PRN231_FinalProject.Interface.Repositories;
using PRN231_FinalProject.Models;

namespace PRN231_FinalProject.Repositories;

public class ContactRepository : GenericRepository<Contact>, IContactRepository
{
    private readonly PRN231_FinalProjectContext _context;
    public ContactRepository(PRN231_FinalProjectContext dbcontext) : base(dbcontext)
    {
        _context = dbcontext;
    }

    public IEnumerable<Contact> GetContactsByUserId(int UserId)
    {
        return _context.Contacts.Include(e => e.ContactEmails).Include(e => e.ContactPhones).Include(e => e.ContactsLabels).ThenInclude(e => e.Label).Where(e => e.UserId == UserId).ToList();
    }

    public Contact ModifyContact(Contact entity)
    {
        var validContactEmails = entity.ContactEmails.Select(e => e.ContactEmailId);
        var missingEmails = _context.ContactEmails.Where(e =>
            e.ContactId == entity.ContactId && !validContactEmails.Contains(e.ContactEmailId));
        _context.ContactEmails.RemoveRange(missingEmails);
        var validContactPhones = entity.ContactPhones.Select(e => e.ContactPhoneId);
        var missingPhones = _context.ContactPhones.Where(e =>
            e.ContactId == entity.ContactId && !validContactPhones.Contains(e.ContactPhoneId));
        _context.ContactPhones.RemoveRange(missingPhones);
        var validLabel = entity.ContactsLabels.Select(e => e.ContactLabelId);
        var missingLabel = _context.ContactsLabels.Where(e =>
            e.ContactId == entity.ContactId && !validLabel.Contains(e.ContactLabelId));
        _context.ContactsLabels.RemoveRange(missingLabel);
        _context.Contacts.Update(entity);
        return entity;
    }

    public List<Contact> ModifyContactRange(List<Contact> entity)
    {
        _context.Contacts.UpdateRange(entity);
        return entity;
    }

    public Contact GetContactById(int Id)
    {
        return _context.Contacts.Include(e => e.ContactEmails).Include(e => e.ContactPhones)
            .Include(e => e.ContactsLabels).ThenInclude(e => e.Label).SingleOrDefault(e => e.ContactId == Id);
    }
    public Contact GetContactByIdWOLabel(int Id)
    {
        return _context.Contacts.Include(e => e.ContactEmails).Include(e => e.ContactPhones)
            .Include(e => e.ContactsLabels).SingleOrDefault(e => e.ContactId == Id);
    }

    public void DeleteTrashContact()
    {
        var trashContact = _context.Contacts.Where(e => e.DeleteDate == DateTime.Now).ToList();
        _context.Contacts.RemoveRange(trashContact);
        _context.SaveChanges();
    }
}