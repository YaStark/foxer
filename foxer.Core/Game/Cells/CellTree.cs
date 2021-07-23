namespace foxer.Core.Game.Cells
{
    public class CellTree : CellBase
    {
        public CellBase SourceCell { get; }

        public CellTree(CellBase sourceCell)
            : base(CellKind.Misc_Tree, sourceCell.X, sourceCell.Y, false)
        {
            SourceCell = sourceCell;
            CanBuildRoad = true;
            BuildRoadPrice = 6;
        }
    }
}
