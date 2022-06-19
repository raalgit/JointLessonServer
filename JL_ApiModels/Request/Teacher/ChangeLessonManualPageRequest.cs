namespace JL_ApiModels.Request.Teacher
{
    [Serializable]
    public class ChangeLessonManualPageRequest : IRequest
    {
        public int CourseId { get; set; }
        public int GroupId { get; set; }
        public string NextPage { get; set; }
        public bool ForMainWindows { get; set; }
    }
}
