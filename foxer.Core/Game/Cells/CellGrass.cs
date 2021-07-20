namespace foxer.Core.Game.Cells
{
    public class CellGrass : CellBase
    {
        public CellBase SourceCell { get; }

        public CellGrass(CellBase sourceCell)
            : base(CellKind.Tree, sourceCell.X, sourceCell.Y, false)
        {
            SourceCell = sourceCell;
            CanBuildRoad = true;
            BuildRoadPrice = 4;
        }
    }
}
