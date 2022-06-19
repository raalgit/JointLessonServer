namespace JL_ApiModels.Request.User
{
    [Serializable]
    public class CreateRemoteAccessRequest : IRequest
    {
        public string ConnectionData { get; set; }
        public int CourseId { get; set; }
    }
}
