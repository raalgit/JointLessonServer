using JL_MSSQLServer;
using JL_MSSQLServer.Repository.Abstraction;
using Microsoft.AspNetCore.SignalR;

namespace JL_SignalR
{
    public class SignalHub : Hub
    {
        private readonly ISignalUserConnectionRepository _signalUserConnectionRepository;
        private readonly ApplicationContext _context;

        public SignalHub(ISignalUserConnectionRepository signalUserConnectionRepository,
                         ApplicationContext context)
        {
            _signalUserConnectionRepository = signalUserConnectionRepository;
            _context = context;
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            deleteConnection(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }

        private void deleteConnection(string connectionId)
        {
            var domain = _signalUserConnectionRepository.Get().FirstOrDefault(x => x.ConnectionId == connectionId);
            if (domain != null)
            {
                _signalUserConnectionRepository.Delete(domain);
                _context.SaveChanges();
            }
        }
    }
}
