using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class BlockPosition
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public BlockPosition(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
