using JL_MSSQLServer.PersistModels;

namespace JL_ApiModels.Response.Editor
{
    public class GetMyMaterialsResponse : ResponseBase, IResponse
    {
        public List<Manual> Manuals { get; set; }
    }
}
