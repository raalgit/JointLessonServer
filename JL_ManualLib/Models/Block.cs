using JL_ManualLib.Models.Interface;

namespace JL_ManualLib.Models
{
    [Serializable]
    public class Block : IBlock
    {
        public string Id { get; set; }
        public int Access { get; set; }
        public int Parts { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
    }
}
