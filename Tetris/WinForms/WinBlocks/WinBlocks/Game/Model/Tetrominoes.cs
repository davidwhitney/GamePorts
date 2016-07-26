using System.Collections.Generic;

namespace WinBlocks.Game.Model
{
    public class Tetrominoes : List<Tetrimino>
    {

        public Tetrominoes()
        {
            Add(new I());
            Add(new O());
            Add(new T());
            Add(new S());
            Add(new Z());
            Add(new J());
            Add(new L());
        }
    }
}