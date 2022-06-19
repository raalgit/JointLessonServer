using JL_ManualLib.Models.Interface;

namespace JL_ManualLib.Models
{
    public class Page : Block, IBlock
    {
        public int Type { get; set; }
        public string DirPath { get; set; }
        public string FileName { get; set; }
        public int FileDataId { get; set; }
        public List<Module>? Modules { get; set; }
    }
}
