namespace FinalProjectMVC.RepositoryPattern
{
    public interface IRepository<T>
        where T : class
    {
        public List<T> GetAll();
        public T? GetDetails<PType>(PType id);
        public void Insert(T Entity);
        public void Update<PType>(PType id, T Entity);
        public void Delete<PType>(PType id);
        public List<T> Filter(Func<T, bool> filterPredicate);

        public Task<List<T>> GetAllAsync();
        public Task<T?> GetDetailsAsync<PType>(PType id);
        public Task InsertAsync(T Entity);
        public Task UpdateAsync<PType>(PType id, T Entity);
        public Task DeleteAsync<PType>(PType id);
        public Task<List<T>> FilterAsync(Func<T, bool> filterPredicate);
       
        public Task <List<T>> Where(Func<T, bool> lambda);
      
    }
}
