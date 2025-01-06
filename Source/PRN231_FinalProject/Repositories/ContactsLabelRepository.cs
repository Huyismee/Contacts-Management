using PRN231_Assignment2_eBookStoreAPI.Repositories.Common;
using PRN231_FinalProject.Interface.Repositories;
using PRN231_FinalProject.Models;

namespace PRN231_FinalProject.Repositories;

public class ContactsLabelRepository : GenericRepository<ContactsLabel>, IContactsLabelRepository
{
    public ContactsLabelRepository(PRN231_FinalProjectContext dbcontext) : base(dbcontext)
    {
    }
}