using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.Auth;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.Auth
{
    public class GetUserByIdPoint : PointBase<int, JL_MSSQLServer.PersistModels.User>, IGetUserByIdPoint
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdPoint(
            IUserRepository _userRepository,
            ApplicationContext _context) : base(_context)
        {
            this._userRepository = _userRepository;
        }

        public override async Task<JL_MSSQLServer.PersistModels.User> Execute(int req, UserSettings userSettings)
        {
            var response = await _userRepository.Get().FirstOrDefaultAsync(x => x.Id == req);
            return response;
        }
    }
}
