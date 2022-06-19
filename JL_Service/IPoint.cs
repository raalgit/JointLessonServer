using JL_Utility.Models;

namespace JL_Service
{
    public interface IPoint<TReq, TResp>
    {
        public Task<TResp> Start(TReq req, UserSettings userSettings);
        public abstract Task<TResp> Execute(TReq req, UserSettings userSettings);
    }
}
