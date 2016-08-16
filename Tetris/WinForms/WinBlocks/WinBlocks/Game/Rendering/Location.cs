namespace WinBlocks.Game.Rendering
{
    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as Location);
        }

        protected bool Equals(Location other)
        {
            return X == other.X && Y == other.Y;
        }

        public Location Mutate(Delta delta)
        {
            X += delta.X;
            Y += delta.Y;
            return this;
        }

        public Location From(Delta delta)
        {
            return new Location {X = X + delta.X, Y = Y + delta.Y};
        }
    }
}