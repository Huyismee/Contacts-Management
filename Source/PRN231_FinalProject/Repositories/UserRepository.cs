using Microsoft.EntityFrameworkCore;

using PRN231_Assignment2_eBookStoreAPI.Repositories.Common;
using PRN231_FinalProject.Interface.Repositories;
using PRN231_FinalProject.Models;


public class UserRepository: GenericRepository<User>, IUserRepository
{
    private readonly PRN231_FinalProjectContext _context;
    public UserRepository(PRN231_FinalProjectContext dbcontext) : base(dbcontext)
    {
        _context = dbcontext;
    }

    public User FindUserByEmail(string email)
    {
        return _context.Users.SingleOrDefault(e => e.Email.Equals(email));

    }


}