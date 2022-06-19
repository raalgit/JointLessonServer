namespace JL_ApiModels.Response.User
{
    public class LoadNoteResponse : ResponseBase, IResponse
    {
        public byte[] File { get; set; }
    }
}
