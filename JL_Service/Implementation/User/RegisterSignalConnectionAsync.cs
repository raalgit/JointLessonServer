using JL_ApiModels.Response.User;
using JL_MSSQLServer;
using JL_MSSQLServer.PersistModels;
using JL_MSSQLServer.Repository.Abstraction;
using JL_Service.Abstraction.User;
using JL_Utility.Models;
using Microsoft.EntityFrameworkCore;

namespace JL_Service.Implementation.User
{
    public class RegisterSignalConnectionAsync : PointBase<string, RegisterSignalConnectionResponse>, IRegisterSignalConnectionAsync
    {
        private readonly ISignalUserConnectionRepository _signalUserConnectionRepository;

        public RegisterSignalConnectionAsync(
            ISignalUserConnectionRepository _signalUserConnectionRepository,
            ApplicationContext _context) : base(_context)
        {
            this._signalUserConnectionRepository = _signalUserConnectionRepository;
        }

        public override async Task<RegisterSignalConnectionResponse> Execute(string req, UserSettings userSettings)
        {
            var response = new RegisterSignalConnectionResponse();

            var oldConnectionData = await _signalUserConnectionRepository.Get()
                .Where(x => x.UserId == userSettings.User.Id).ToListAsync();

            if (oldConnectionData != null && oldConnectionData.Count > 0)
            {
                foreach (var data in oldConnectionData) _signalUserConnectionRepository.Delete(data);
            }

            var domain = new SignalUserConnection()
            {
                UserId = userSettings.User.Id,
                ConnectionId = req
            };

            _signalUserConnectionRepository.Insert(domain);
            response.Message = $"SignalR соединение успешно зарегистрировано (...{req[..5]})";
            return response;
        }
    }
}
