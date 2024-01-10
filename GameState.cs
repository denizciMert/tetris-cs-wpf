using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class GameState
    {
        private GameBlocks currentBlock;

        public GameBlocks CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();

                for (int i=0; i<2;i++)
                {
                    currentBlock.MoveBlock(1, 0);

                    if (!BlockFits())
                    {
                        currentBlock.MoveBlock(-1,0);
                    }
                }
            }
        }

        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public GameBlocks HeldBlock { get; private set; }
        public bool CanHold {  get; private set; }


        public GameState()
        {
            GameGrid = new GameGrid(22,10);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate();
            CanHold = true;
        }

        private bool BlockFits()
        {
            foreach (BlockPosition p in CurrentBlock.TilesPositions())
            {
                if (!GameGrid.IsEmptyGrid(p.Row,p.Column))
                {
                    return false;
                }
            }
            return true;
        }

        public void HoldBlock() 
        {
            if (!CanHold)
            {
                return;
            }

            if (HeldBlock== null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();
            }
            else
            {
                GameBlocks tmp = CurrentBlock;
                CurrentBlock = HeldBlock;
                HeldBlock = tmp;
            }

            CanHold = false;
        }

        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCCW();
            }
        }

        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();

            if (!BlockFits())
            {
                CurrentBlock.RotateCW();
            }
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.MoveBlock(0,-1);

            if (!BlockFits())
            {
                CurrentBlock.MoveBlock(0, 1);
            }
        }

        public void MoveBlockRight()
        {
            CurrentBlock.MoveBlock(0, 1);

            if (!BlockFits())
            {
                CurrentBlock.MoveBlock(0, -1);
            }
        }

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmptyGrid(0) && GameGrid.IsRowEmptyGrid(1));
        }

        private void PlaceBlock()
        {
            foreach (BlockPosition p in CurrentBlock.TilesPositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }

            Score += GameGrid.ClearFullRows();

            if (IsGameOver())
            {
                GameOver = true;
            }
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate();
                CanHold = true;
            }
        }

        public void MoveBlockDowm()
        {
            CurrentBlock.MoveBlock(1,0);

            if (!BlockFits())
            {
                CurrentBlock.MoveBlock(-1,0);
                PlaceBlock();
            }
        }

        private int TileDropDistance(BlockPosition p)
        {
            int drop = 0;

            while (GameGrid.IsEmptyGrid(p.Row+drop+1,p.Column))
            {
                drop++;
            }

            return drop;
        }

        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;
            foreach (BlockPosition p in CurrentBlock.TilesPositions())
            {
                drop = System.Math.Min(drop,TileDropDistance(p));
            }

            return drop;
        }

        public void DropBlock()
        {
            CurrentBlock.MoveBlock(BlockDropDistance(), 0);
            PlaceBlock() ;
        }
    }
}
