using foxer.Core.Game.Cells;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Core.Game.Generator.StageGenerator
{
    public class StageGeneratorArgs
    {
        public Random Rnd { get; }
        public IList<CellDoor> Doors { get; private set; }
        public Point? PlayerSpawnLocation { get; set; }

        public StageGeneratorArgs(Random rnd, params CellDoor[] doors)
        {
            Rnd = rnd;
            Doors = new List<CellDoor>(doors);
        }
    }
}
