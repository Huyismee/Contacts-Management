using Microsoft.EntityFrameworkCore;
using PRN231_FinalProject.Interface.Repositories.Common;
using PRN231_FinalProject.Models;

namespace PRN231_FinalProject.Interface.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    public User FindUserByEmail(string email);
}