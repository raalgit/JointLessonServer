namespace JL_ApiModels.Request.Teacher
{
    [Serializable]
    public class CloseLessonRequest : IRequest
    {
        public int CourseId { get; set; }
    }
}
