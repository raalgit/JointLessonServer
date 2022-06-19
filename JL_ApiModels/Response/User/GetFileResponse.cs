namespace JL_ApiModels.Response.User
{
    public class GetFileResponse : ResponseBase, IResponse
    {
        public byte[] File { get; set; }
    }
}
