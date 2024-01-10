using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class GameGrid
    {
        private readonly int[,] grid;
        public int Rows { get; }
        public int Columns { get; }

        public int this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[rows, columns];
        }

        public bool IsInsideGrid(int r, int c)
        {
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }

        public bool IsEmptyGrid(int r, int c)
        {
            return IsInsideGrid(r, c) && grid[r, c] == 0;
        }

        public bool IsRowFullGrid(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsRowEmptyGrid(int r)
        {
            for (int c = 0; c< Columns; c++)
            {
                if (grid[r, c] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void ClearRows(int r)
        {
            for(int c = 0;c< Columns; c++)
            {
                grid[r, c] = 0;
            }
        }

        private void MoveDownRows(int r, int MoveDownCount)
        {
            for(int c = 0;c < Columns; c++)
            {
                grid[r+ MoveDownCount, c] = grid[r,c];
                grid[r,c] = 0;
            }
        }

        public int ClearFullRows()
        {
            int cleared= 0;

            for(int r= Rows-1; r>=0; r--)
            {
                if (IsRowFullGrid(r))
                {
                    ClearRows(r);
                    cleared++;
                }
                else if (cleared>0)
                {
                    MoveDownRows(r,cleared);
                }
            }

            return cleared;
        }
    }
}
