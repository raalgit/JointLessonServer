namespace JL_ApiModels.Response.Teacher
{
    public class StartSyncLessonResponse : ResponseBase, IResponse
    {
        public bool CanConnectToSyncLesson { get; set; }
    }
}
