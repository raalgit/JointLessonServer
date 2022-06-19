using JL_ManualLib.Models;

namespace JL_ApiModels.Request.Editor
{
    [Serializable]
    public class UpdateMaterialRequest : IRequest
    {
        public string OriginalName { get; set; }
        public int OriginalFileDataId { get; set; }
        public ManualData? ManualData { get; set; }
    }
}
