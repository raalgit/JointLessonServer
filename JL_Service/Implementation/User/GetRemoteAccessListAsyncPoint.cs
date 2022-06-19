using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.User
{
    public class GetRemoteAccessListAsyncPoint : PointBase<int, GetRemoteAccessListResponse>, IGetRemoteAccessListAsyncPoint
    {
        private IUserRemoteAccessRepository _userRemoteAccessRepository;
        private IUserRepository _userRepository;
        private const int sessionMinuteTimeLimit = 20;

        public GetRemoteAccessListAsyncPoint(
            IUserRemoteAccessRepository _userRemoteAccessRepository,
            IUserRepository _userRepository,
            ApplicationContext _context) : base(_context)
        {
            this._userRemoteAccessRepository = _userRemoteAccessRepository;
            this._userRepository = _userRepository;
        }

        public override async Task<GetRemoteAccessListResponse> Execute(int req, UserSettings userSettings)
        {
            var response = new GetRemoteAccessListResponse();

            var getRemoteAccessListQuery = from remote in _userRemoteAccessRepository.Get()
                                           join user in _userRepository.Get() on remote.UserId equals user.Id
                                           where 
                                           remote.CourseId == req && 
                                           remote.StartDate.AddMinutes(sessionMinuteTimeLimit) < DateTime.Now
                                           select new UserRemoteAccessWithUserData()
                                           {
                                               UserRemote = remote,
                                               UserName = user.FirstName + " " + user.ThirdName
                                           };

            List<UserRemoteAccessWithUserData> remoteAccessList = await getRemoteAccessListQuery.ToListAsync();
            response.UserRemoteAccesses = remoteAccessList;
            response.ShowMessage = true;
            response.Message = $"Список удаленных терминалов получен ({remoteAccessList.Count} шт.)";
            return response;
        }
    }
}
