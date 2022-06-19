namespace JL_SignalR
{
    public interface ISignalRUtility
    {
        public Task SendMessage(SignalType type, string message, string connectionId);
        public Task<bool> SendLessonStateSignalR(int courseId);
    }
}
