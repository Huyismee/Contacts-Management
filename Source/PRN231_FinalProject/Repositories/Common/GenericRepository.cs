using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN231_FinalProject.Interface.Repositories.Common;
using PRN231_FinalProject.Models;

namespace PRN231_Assignment2_eBookStoreAPI.Repositories.Common
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly PRN231_FinalProjectContext _dbContext;
        private DbSet<T> _dbSet;

        public GenericRepository(PRN231_FinalProjectContext dbcontext)
        {
            _dbContext = dbcontext;
            _dbSet = dbcontext.Set<T>();
        }

        public T Add(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            return entity;
        }

        public List<T> AddRange(List<T> entity)
        {
            _dbContext.Set<T>().AddRange(entity);
            return entity;
        }

        public T Update(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }
        public T Modify(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }
        public void DeleteRange(List<T> entity)
        {
            _dbContext.Set<T>().RemoveRange(entity);
        }

        public T GetById(int id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = _dbSet;
            return _dbSet.AsNoTracking()
                .ToList();
        }
    }
}
