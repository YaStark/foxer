namespace foxer.Core.Game.Cells
{
    public abstract class CellBase
    {
        public bool CanBuildRoad { get; protected set; }

        public int BuildRoadPrice { get; protected set; }

        public bool CanWalk { get; protected set; }

        public int Rotation { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public CellKind Kind { get; }

        protected CellBase(CellKind kind, int x, int y, bool walkable)
        {
            Kind = kind;
            CanWalk = walkable;
            CanBuildRoad = walkable;
            BuildRoadPrice = CanBuildRoad ? 1 : int.MaxValue;
            X = x;
            Y = y;
        }
    }

    public enum CellKind
    {
        Bound,
        Floor,
        Water,
        Door,
        Stair,
        Road,
        Tree
    }
}
