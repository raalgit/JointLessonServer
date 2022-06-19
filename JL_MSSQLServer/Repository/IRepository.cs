namespace JL_MSSQLServer.Repository
{
    public interface IRepository<TModel> : IDisposable where TModel : class, IPersist
    {
        TModel Insert(TModel persist);
        IEnumerable<TModel> GetAll();
        IQueryable<TModel> Get();
        TModel GetById(int id);
        TModel Update(TModel persist);
        void UpdateMany(IEnumerable<TModel> entities);
        void Delete(TModel persist);
        void DeleteById(int id);

        Task<TModel> InsertAsync(TModel persist);
        Task<TModel> GetByIdAsync(int id);
        Task<TModel> UpdateAsync(TModel persist);
    }
}
