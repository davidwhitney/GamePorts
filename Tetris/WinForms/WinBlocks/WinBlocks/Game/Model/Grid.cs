using System;
using System.Collections.Generic;

namespace WinBlocks.Game.Model
{
    public class Grid<T> where T : class
    {
        private readonly Func<T> _defaultCell;
        public int Width { get; set; }
        public int Height { get; set; }

        protected List<List<T>> Storage;

        public Grid(int width, int height, Func<T> defaultCell = null)
        {
            _defaultCell = defaultCell ?? (() => null);
            Width = width;
            Height = height;
            Storage = InitiliseStorage();
        }

        public List<List<T>> RawRows => Storage;
        
        public T ValueAt(int x, int y)
        {
            if (x < 0 || y < 0 || x > Width || y > Height)
            {
                return null;
            }

            return Storage[y][x];
        }

        public virtual bool IsOccupied(int x, int y)
        {
            if (x < 0 || y < 0)
            {
                return false;
            }

            if (y >= Height)
            {
                return false;
            }

            if (x >= Width)
            {
                return false;
            }

            return Storage[y][x] != null;
        }

        public void SetValue(int x, int y, T val)
        {
            Storage[y][x] = val;
        }
        
        protected List<List<T>> InitiliseStorage(bool copy = false)
        {
            var s = new List<List<T>>();
            for (var y = 0; y < Height; y++)
            {
                s.Add(CreateRow(y, copy));
            }
            return s;
        }

        public List<T> CreateRow(int y = 0, bool copy = false)
        {
            var row = new List<T>();
            for (var x = 0; x < Width; x++)
            {
                var cell = _defaultCell();
                if (copy)
                {
                    cell = ValueAt(x, y);
                }
                row.Add(cell);
            }
            return row;
        }
    }
}