namespace JL_ApiModels.Request.User
{
    public class GetManualFilesRequest : IRequest
    {
        public List<int> FileDataIds { get; set; }
    }
}
