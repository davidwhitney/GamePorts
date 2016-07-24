namespace WinBlocks
{
    public class RenderLocation
    {
        public int X { get; set; }
        public int Y { get; set; }
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
            return X == other.X && Y == other.Y && Content == other.Content;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode*397) ^ Y.GetHashCode();
                hashCode = (hashCode*397) ^ Content.GetHashCode();
                return hashCode;
            }
        }
    }
}