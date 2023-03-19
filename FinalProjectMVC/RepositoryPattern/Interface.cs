namespace FinalProjectMVC.RepositoryPattern
{
    public interface IRepository<T>
        where T : class
    {
        public List<T> GetAll();
        public T? GetDetails(int? id);
        public void Insert(T Entity);
        public void Update(int? id, T Entity);
        public void Delete(int? id);
    }
}
