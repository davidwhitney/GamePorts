using System;
using System.Collections.Generic;
using static System.Environment;

namespace WinBlocks.Game.Model
{
    public class Tetrominoes : List<Tetrimino>
    {

        public Tetrominoes()
        {
            var nl = NewLine;

            Add(new Tetrimino("...." + nl +
                              "IIII" + nl +
                              "...." + nl +
                              "....",
                rotationStates: new[]{"..I.\r\n..I.\r\n..I.\r\n..I.", "....\r\n....\r\nIIII\r\n....", ".I..\r\n.I..\r\n.I..\r\n.I.."}));

            Add(new Tetrimino(".OO." + nl +
                              ".OO." + nl +
                              "...."));
            
            Add(new Tetrimino(".T." + nl +
                              "TTT" + nl +
                              "...",
              rotationStates: new[] { ".T.\r\n.TT\r\n.T.", "...\r\nTTT\r\n.T.", ".T.\r\nTT.\r\n.T." }));

            Add(new Tetrimino(".SS" + nl +
                              "SS." + nl +
                              "...",
              rotationStates: new[] { ".S.\r\n.SS\r\n..S", "...\r\n.SS\r\nSS.", "S..\r\nSS.\r\n.S." }));

            Add(new Tetrimino("ZZ." + nl +
                              ".ZZ" + nl +
                              "...",
              rotationStates: new[] { "..Z\r\n.ZZ\r\n.Z", "...\r\nZZ.\r\n.ZZ", ".Z.\r\nZZ.\r\nZ.." }));

            Add(new Tetrimino("J.." + nl +
                              "JJJ" + nl +
                              "...",
              rotationStates: new[] { ".JJ\r\n.J.\r\n.J.", "...\r\nJJJ\r\n..J", ".J.\r\n.J.\r\nJJ." }));
            
            Add(new Tetrimino("..L" + nl +
                              "LLL" + nl +
                              "...",
              rotationStates: new[] { ".L.\r\n.L.\r\n.LL", "...\r\nLLL\r\nL..", "LL.\r\n.L.\r\n.L." }));
        }
    }
}