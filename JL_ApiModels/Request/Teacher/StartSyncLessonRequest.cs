namespace JL_ApiModels.Request.Teacher
{
    [Serializable]
    public class StartSyncLessonRequest : IRequest
    {
        public string StartPage { get; set; }
        public int CourseId { get; set; }
        public int GroupId { get; set; }
    }
}
