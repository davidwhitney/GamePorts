using WinBlocks.Game.Model;

namespace WinBlocks.Game.Rendering
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

        public override string ToString()
        {
            return Content;
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