namespace JL_ApiModels.Response.Teacher
{
    public class CloseLessonResponse : ResponseBase, IResponse
    {
        public bool CanConnectToSyncLesson { get; set; }
    }
}
