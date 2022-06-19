using JL_ManualLib.Models;

namespace JL_ApiModels.Response.Editor
{
    public class GetMaterialResponse : ResponseBase, IResponse
    {
        public string OriginalName { get; set; }
        public ManualData ManualData { get; set; }
    }
}
