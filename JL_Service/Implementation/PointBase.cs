using JL_MSSQLServer;
using JL_Utility.Models;

namespace JL_Service.Implementation
{
    public abstract class PointBase<TReq, TRes> : IPoint<TReq, TRes>
    {
        private readonly ApplicationContext _context;

        public PointBase(ApplicationContext _context)
        {
            this._context = _context;
        }

        public Task<TRes> Start(TReq req, UserSettings userSettings)
        {
            var resp = Execute(req, userSettings);

            _context.SaveChanges();
            return resp;
        }

        public abstract Task<TRes> Execute(TReq req, UserSettings userSettings);
    }
}
