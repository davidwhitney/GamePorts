namespace ConsoleApplication1.GameModel
{
    public class VocabularyItem
    {
        public string Word { get; set; }
        public bool IsMotion { get; set; }
        public bool IsObject { get; set; }
        public bool IsAction { get; set; }
        public bool IsSpecial { get; set; }
        public int SpecialIndex { get; set; }
        public bool IsTreasure { get; set; }
    }
}