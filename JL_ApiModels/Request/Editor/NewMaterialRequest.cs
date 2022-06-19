using JL_ManualLib.Models;

namespace JL_ApiModels.Request.Editor
{
    [Serializable]
    public class NewMaterialRequest : IRequest
    {
        public string OriginalName { get; set; }
        public ManualData? ManualData { get; set; }
    }
}
