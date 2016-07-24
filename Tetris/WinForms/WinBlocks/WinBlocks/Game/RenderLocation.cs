namespace WinBlocks.Game
{
    public class RenderLocation : Location
    {

        public string Content { get; set; }

        public RenderLocation(int x, int y, string content = ".")
        {
            X = x;
            Y = y;
            Content = content;
        }

        public RenderLocation()
        {
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RenderLocation);
        }

        protected bool Equals(RenderLocation other)
        {
            return base.Equals(other) && Content == other.Content;
        }
    }
}