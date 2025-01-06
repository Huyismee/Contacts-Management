using Microsoft.EntityFrameworkCore;
using PRN231_Assignment2_eBookStoreAPI.Repositories.Common;
using PRN231_FinalProject.Interface.Repositories;
using PRN231_FinalProject.Models;

namespace PRN231_FinalProject.Repositories;

public class LabelRepository : GenericRepository<Label>, ILabelRepository
{
    private readonly PRN231_FinalProjectContext _context;
    public LabelRepository(PRN231_FinalProjectContext dbcontext) : base(dbcontext)
    {
        _context = dbcontext;
    }

    public IEnumerable<Label> GetLabelsByUserId(int UserId)
    {
        return _context.Labels.Where(e => e.UserId == UserId).ToList();
    }
}