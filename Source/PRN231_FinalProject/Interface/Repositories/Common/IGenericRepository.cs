namespace PRN231_FinalProject.Interface.Repositories.Common;


public interface IGenericRepository<T> where T : class
{
    T Add(T entity);
    T Update(T entity);
    void Delete(T entity);
    T GetById(int id);
    public T Modify(T entity);
    public List<T> AddRange(List<T> entity);
    public void DeleteRange(List<T> entity);
    IEnumerable<T> GetAll();
}
