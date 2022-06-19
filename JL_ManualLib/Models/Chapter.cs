using JL_ManualLib.Models.Interface;

namespace JL_ManualLib.Models
{
    [Serializable]
    public class Chapter : Block, IBlock
    {
        public List<Topic>? Topics { get; set; }
    }
}
