using System;
using System.Collections.Generic;
using System.Linq;

namespace WinBlocks.Game
{
    public class Grid<T> where T : class
    {
        public int Width { get; set; }
        public int Height { get; set; }

        protected readonly List<List<T>> Storage;

        public Grid(int width, int height)
        {
            Width = width;
            Height = height;

            Storage = new List<List<T>>();
            var yCells = Enumerable.Repeat((List<T>) null, height).ToList();
            yCells.AddRange(Enumerable.Repeat((List<T>)null, height));
            Storage.AddRange(yCells);
        }

        public T ValueAt(int x, int y)
        {
            return Storage[y][x];
        }

        public void SetValue(int x, int y, T val)
        {
            Storage[y][x] = val;
        }

    }
}