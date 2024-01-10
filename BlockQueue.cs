using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class BlockQueue
    {
        private readonly GameBlocks[] gameBlocks = new GameBlocks[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        private readonly Random random = new Random();

        public GameBlocks NextBlock { get; private set; }

        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }

        private GameBlocks RandomBlock()
        {
            return gameBlocks[random.Next(gameBlocks.Length)];
        }

        public GameBlocks GetAndUpdate()
        {
            GameBlocks gameBlocks = NextBlock;

            do
            {
                NextBlock = RandomBlock();
            }
            while (gameBlocks.Id == NextBlock.Id);

            return gameBlocks;
        }
    }
}
