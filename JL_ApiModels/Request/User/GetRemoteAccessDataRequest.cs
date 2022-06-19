namespace JL_ApiModels.Request.User
{
    [Serializable]
    public class GetRemoteAccessDataRequest : IRequest
    {
        public int CourseId { get; set; }
        public int UserId { get; set; }
    }
}
