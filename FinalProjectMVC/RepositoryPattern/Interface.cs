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
    }
}
