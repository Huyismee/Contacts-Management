using PRN231_FinalProject.Interface.Repositories.Common;
using PRN231_FinalProject.Models;

namespace PRN231_FinalProject.Interface.Repositories;

public interface ILabelRepository : IGenericRepository<Label>
{
    public IEnumerable<Label> GetLabelsByUserId(int UserId);
}