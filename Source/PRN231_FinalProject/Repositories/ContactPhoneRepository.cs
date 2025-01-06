using Microsoft.EntityFrameworkCore;
using PRN231_Assignment2_eBookStoreAPI.Repositories.Common;
using PRN231_FinalProject.Interface.Repositories;
using PRN231_FinalProject.Models;

namespace PRN231_FinalProject.Repositories;

public class ContactPhoneRepository : GenericRepository<ContactPhone>, IContactPhoneRepository
{
    private readonly PRN231_FinalProjectContext _context;
    public ContactPhoneRepository(PRN231_FinalProjectContext dbcontext) : base(dbcontext)
    {
        _context = dbcontext;
    }

}