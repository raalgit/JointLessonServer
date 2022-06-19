namespace JL_ApiModels.Request.User
{
    [Serializable]
    public class SendNoteRequest : IRequest
    {
        public byte[] File { get; set; }
        public int CourseId { get; set; }
    }
}
