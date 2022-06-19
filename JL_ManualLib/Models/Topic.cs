using JL_ManualLib.Models.Interface;

namespace JL_ManualLib.Models
{
    public class Topic : Block, IBlock
    {
        public List<DidacticUnit>? DidacticUnits { get; set; }
    }
}
