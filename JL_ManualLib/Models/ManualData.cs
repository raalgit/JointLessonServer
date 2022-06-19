using JL_ManualLib.Models.Interface;

namespace JL_ManualLib.Models
{
    [Serializable]
    public class ManualData : Block, IBlock
    {
        public List<Author> Authors { get; set; }
        public MaterialDate? MaterialDate { get; set; }
        public string Discipline { get; set; }

        public List<Chapter> Chapters { get; set; }
    }
}
