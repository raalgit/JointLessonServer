namespace JL_ApiModels.Request.User
{
    public class AddNewFileRequest : IRequest
    {
        public byte[] File { get; set; }
        public string Name { get; set; }
    }
}
