namespace JL_ApiModels.Request.User
{
    [Serializable]
    public class UpHandRequest : IRequest
    {
        public int CourseId { get; set; }
    }
}
