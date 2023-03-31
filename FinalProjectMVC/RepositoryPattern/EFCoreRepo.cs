using FinalProjectMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectMVC.RepositoryPattern
{
    public abstract class EFCoreRepo<T> : IRepository<T>
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

        public virtual void Update<PType>(PType id, T entity)
        {
            var existingEntity = _context.Set<T>().Find(id);
            if (existingEntity is not null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                _context.SaveChanges();
            }
        }

        public virtual void Delete<PType>(PType id)
        {
            var existingEntity = _context.Set<T>().Find(id);
            if (existingEntity is not null)
            {
                _context.Set<T>().Remove(existingEntity);
                _context.SaveChanges();
            }
        }

        public virtual T? GetDetails<PType>(PType id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        
        public virtual List<T> Filter(Func<T, bool> filterPredicate)
        {
            return _context.Set<T>().Where(filterPredicate).ToList();
        }

        public async virtual Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async virtual Task<T?> GetDetailsAsync<PType>(PType id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async virtual Task InsertAsync(T Entity)
        {
            await _context.Set<T>().AddAsync(Entity);
            await _context.SaveChangesAsync();
        }

        public async virtual Task UpdateAsync<PType>(PType id, T Entity)
        {
            var existingEntity = await _context.Set<T>().FindAsync(id);
            if (existingEntity is not null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(Entity);
                await _context.SaveChangesAsync();
            }
        }

        public async virtual Task DeleteAsync<PType>(PType id)
        {
            var existingEntity = await _context.Set<T>().FindAsync(id);
            if (existingEntity is not null)
            {
                _context.Set<T>().Remove(existingEntity);
                await _context.SaveChangesAsync();
            }
        }
        public async virtual Task <List<T>>  Where(Func<T, bool> lambda)
        {
            return  _context.Set<T>().Where(lambda).ToList();
        }

        public async virtual Task<List<T>> FilterAsync(Func<T, bool> filterPredicate)
        {
            return await Task.FromResult(_context.Set<T>().Where(filterPredicate).ToList());
        }

        //public async Task<List<T>> GetReviews(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new List<T>();
        //    }

        //    var reviews = await _context.Set<T>().FilterAsync(r => r.ProductId == ProductId);
        //    //var reviews = await _context.Set<T>().ToListAsync();

        //    return reviews;
        //}
      
        /* The filter method is added so we don't have to add where and tolist() to everything
         * 
         1- Task => used to allowe tolist() to work Async 

        2- Func<T, bool> ==> delegate that allow us to send a `lamda expression` 
            as where returns bool (true or false) based on if the record meets the condition or not

            If it's a match, we add the record to our list and We continue. 

        3- Set<T> is used to specify a `DBSet` type when using generics. 


        */

    }
}
