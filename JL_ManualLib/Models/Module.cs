using JL_ManualLib.Models.Interface;

namespace JL_ManualLib.Models
{
    public class Module : Block, IBlock
    {
        public int FileDataId { get; set; }
        public int Type { get; set; }
    }
}
