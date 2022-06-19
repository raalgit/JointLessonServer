using Microsoft.EntityFrameworkCore;

namespace JL_MSSQLServer.Repository
{
    public class RepositoryBase<TModel> : IRepository<TModel> where TModel : class, IPersist
    {
        private ApplicationContext _context;

        public RepositoryBase(ApplicationContext context)
        {
            _context = context;
        }

        private DbSet<TModel> getDbSet() => _context.Set<TModel>();

        public void Delete(TModel persist)
        {
            _context.Remove(persist);
        }

        public void DeleteById(int id)
        {
            var persist = getDbSet().Where(x => x.Id == id).FirstOrDefault();
            if (persist != null) _context.Remove(persist);
        }

        public IQueryable<TModel> Get()
        {
            IQueryable<TModel> query = _context.Set<TModel>();
            return query;
        }

        public IEnumerable<TModel> GetAll()
        {
            return getDbSet();
        }

        public TModel? GetById(int id)
        {
            return getDbSet().FirstOrDefault(x => x.Id == id);
        }

        public TModel Insert(TModel persist)
        {
            return _context.Add(persist).Entity;
        }

        public TModel Update(TModel persist)
        {
            return (TModel)_context.Update(persist).Entity;
        }

        public void UpdateMany(IEnumerable<TModel> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public async Task<TModel> InsertAsync(TModel persist)
        {
            var resp = await _context.AddAsync(persist);
            return resp.Entity;
        }

        public async Task<TModel> GetByIdAsync(int id)
        {
            return await getDbSet().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TModel> UpdateAsync(TModel persist)
        {
            var resp = await Task.Run(() => { 
                return (TModel)_context.Update(persist).Entity; 
            });
            return resp;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
