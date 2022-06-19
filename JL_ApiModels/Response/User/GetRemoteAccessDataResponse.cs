namespace JL_ApiModels.Response.User
{
    public class GetRemoteAccessDataResponse : ResponseBase, IResponse
    {
        public string ConnectionData { get; set; }
    }
}
