using JL_ApiModels.Request.User;
using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.User
{
    public class GetRemoteAccessDataAsyncPoint : PointBase<GetRemoteAccessDataRequest, GetRemoteAccessDataResponse>, IGetRemoteAccessDataAsyncPoint
    {
        private IUserRemoteAccessRepository _userRemoteAccessRepository;
        public GetRemoteAccessDataAsyncPoint(
            IUserRemoteAccessRepository _userRemoteAccessRepository,
            ApplicationContext _context) : base(_context)
        {
            this._userRemoteAccessRepository = _userRemoteAccessRepository;
        }

        public override async Task<GetRemoteAccessDataResponse> Execute(GetRemoteAccessDataRequest req, UserSettings userSettings)
        {
            var response = new GetRemoteAccessDataResponse();

            var data = await _userRemoteAccessRepository.Get().FirstOrDefaultAsync(x => 
                x.CourseId == req.CourseId && 
                x.UserId == req.UserId && 
                (DateTime.Now - x.StartDate).TotalHours < 1
                );

            response.ConnectionData = data != null ? data.ConnectionData : String.Empty;
            response.Message = "Данные для удаленного подключения получены";
            return response;
        }
    }
}
