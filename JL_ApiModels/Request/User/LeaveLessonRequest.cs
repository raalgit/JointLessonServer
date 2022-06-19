namespace JL_ApiModels.Request.User
{
    [Serializable]
    public class LeaveLessonRequest : IRequest
    {
        public int CourseId { get; set; }
    }
}
