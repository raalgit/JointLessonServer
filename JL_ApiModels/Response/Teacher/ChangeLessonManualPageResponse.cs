namespace JL_ApiModels.Response.Teacher
{
    public class ChangeLessonManualPageResponse : ResponseBase, IResponse
    {
        public string NewPage { get; set; }
        public string IsOnline { get; set; }
    }
}
