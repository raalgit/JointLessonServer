namespace JL_ApiModels.Request.User
{
    [Serializable]
    public class CloseSRSLessonRequest : IRequest
    {
        public int CourseId { get; set; }
    }
}
