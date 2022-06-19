namespace JL_ApiModels.Request.User
{
    [Serializable]
    public class ChangeSRSLessonManualPageRequest : IRequest
    {
        public int CourseId { get; set; }
        public string NextPage { get; set; }
    }
}
