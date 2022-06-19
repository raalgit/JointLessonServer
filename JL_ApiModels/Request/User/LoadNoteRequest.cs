namespace JL_ApiModels.Request.User
{
    [Serializable]
    public class LoadNoteRequest : IRequest
    {
        public int CourseId { get; set; }
    }
}
