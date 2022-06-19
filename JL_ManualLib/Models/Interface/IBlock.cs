namespace JL_ManualLib.Models.Interface
{
    public interface IBlock
    {
        public string Id { get; set; }
        public int Access { get; set; }
        public int Parts { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
    }
}
