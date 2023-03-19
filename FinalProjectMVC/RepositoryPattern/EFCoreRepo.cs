using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public class EFCoreRepo<T> : IRepository<T>
        where T : class
    {
        protected readonly DbContext _context;

        public EFCoreRepo(DbContext context)
        {
            _context = context;
        }

        public virtual void Insert(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public virtual void Update(int? id, T entity)
        {
            var existingEntity = _context.Set<T>().Find(id);
            if (existingEntity is not null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
        }

        public virtual void Delete(int? id)
        {
            var existingEntity = _context.Set<T>().Find(id);
            if (existingEntity is not null)
            {
                _context.Set<T>().Remove(existingEntity);
                _context.SaveChanges();
            }
        }

        public virtual T? GetDetails(int? id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
    }
}
