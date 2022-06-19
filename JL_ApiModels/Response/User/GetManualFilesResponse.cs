using JL_MSSQLServer.PersistModels;

namespace JL_ApiModels.Response.User
{
    public class GetManualFilesResponse : ResponseBase, IResponse
    {
        public List<FileData> FileDatas { get; set; }
    }
}
