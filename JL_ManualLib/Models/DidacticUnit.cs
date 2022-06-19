using JL_ManualLib.Models.Interface;

namespace JL_ManualLib.Models
{
    public class DidacticUnit : Block, IBlock
    {
        public List<Page>? Pages { get; set; }
    }
}
