using PRN231_FinalProject.Interface.Repositories.Common;
using PRN231_FinalProject.Models;

namespace PRN231_FinalProject.Interface.Repositories;

public interface IContactRepository : IGenericRepository<Contact>
{
    public IEnumerable<Contact> GetContactsByUserId(int UserId);

    public Contact ModifyContact(Contact entity);

    public Contact GetContactById(int UserId);
    public Contact GetContactByIdWOLabel(int Id);
    public List<Contact> ModifyContactRange(List<Contact> entity);
    public void DeleteTrashContact();
}