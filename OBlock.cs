﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public class OBlock : GameBlocks
    {
        private readonly BlockPosition[][] tiles = new BlockPosition[][]
        {
            new BlockPosition[]{new(0,0), new(0,1), new(1,0), new(1,1)}
        };

        public override int Id => 4;
        protected override BlockPosition StartOffset => new BlockPosition(0, 4);
        protected override BlockPosition[][] Tiles => tiles;
    }
}
