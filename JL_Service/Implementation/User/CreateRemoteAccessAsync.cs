using JL_ApiModels.Request.User;
using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.User
{
    public class CreateRemoteAccessAsync : PointBase<CreateRemoteAccessRequest, CreateRemoteAccessResponse>, ICreateRemoteAccessAsync
    {
        private readonly IUserRemoteAccessRepository _userRemoteAccessRepository;

        public CreateRemoteAccessAsync(
            IUserRemoteAccessRepository _userRemoteAccessRepository,
            ApplicationContext _context) : base(_context)
        {
            this._userRemoteAccessRepository = _userRemoteAccessRepository;
        }

        public override async Task<CreateRemoteAccessResponse> Execute(CreateRemoteAccessRequest req, UserSettings userSettings)
        {
            var response = new CreateRemoteAccessResponse();

            var oldData = await _userRemoteAccessRepository.Get().FirstOrDefaultAsync(x => x.UserId == userSettings.User.Id);
            if (oldData != null)
            {
                _userRemoteAccessRepository.Delete(oldData);
            }

            await _userRemoteAccessRepository.InsertAsync(new UserRemoteAccess()
            {
                ConnectionData = req.ConnectionData,
                StartDate = DateTime.Now,
                UserId = userSettings.User.Id,
                CourseId = req.CourseId
            });

            response.Message = "Удаленный доступ синхронизирован. К Вам можно подключиться";
            return response;
        }
    }
}
