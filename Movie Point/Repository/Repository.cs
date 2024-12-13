using Microsoft.EntityFrameworkCore;
using Movie_Point.Data;
using System.Linq.Expressions;
using System.Linq;
using Movie_Point.Repository.IRepository;
using Microsoft.EntityFrameworkCore.Query;

namespace Movie_Point.Repositroy
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private DbSet<T> _dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public void Create(T entity)
        {
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Alter(T entity)
        {
            _dbSet.Update(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
        }
        public void Saving()
        {
            _dbContext.SaveChanges();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProps = null,
             bool tracked = true)
        {
            IQueryable<T> query = _dbSet;
            
            if (includeProps != null)
            {  
                    query = includeProps(query); 
            }
            

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return query;
        }

        

        public T? GetOne(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IIncludableQueryable<T, object>>? includeProps = null, bool tracked = true)
        {
            return Get(filter , includeProps , tracked).FirstOrDefault();
        }
    }
}
