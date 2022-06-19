using JL_MSSQLServer;
using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.Auth;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.Auth
{
    public class GetRolesByUserIdPoint : PointBase<int, List<Role>>, IGetRolesByUserIdPoint
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;

        public GetRolesByUserIdPoint(
            ApplicationContext _context,
            IUserRepository _userRepository,
            IUserRoleRepository _userRoleRepository,
            IRoleRepository _roleRepository) : base(_context)
        {
            this._userRoleRepository = _userRoleRepository;
            this._userRepository = _userRepository;
            this._roleRepository = _roleRepository;
        }

        public override async Task<List<Role>> Execute(int req, UserSettings userSettings)
        {
            var query = from user in _userRepository.Get()
                        join user_role in _userRoleRepository.Get() on user.Id equals user_role.UserId
                        join role in _roleRepository.Get() on user_role.RoleId equals role.Id
                        where user.Id == req
                        select role;

            return await query.ToListAsync();
        }
    }
}
