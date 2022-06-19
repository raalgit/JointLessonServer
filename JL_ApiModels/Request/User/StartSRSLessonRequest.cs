namespace JL_ApiModels.Request.User
{
    [Serializable]
    public class StartSRSLessonRequest : IRequest
    {
        public int CourseId { get; set; }
    }
}
