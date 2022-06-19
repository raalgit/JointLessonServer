namespace JL_ApiModels.Request.User
{
    [Serializable]
    public class JoinLessonRequest : IRequest
    {
        public int CourseId { get; set; }
    }
}
