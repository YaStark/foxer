namespace foxer.Core.Game.Cells
{
    public class CellDoor : CellBase
    {
        public DoorKind DoorKind { get; }

        public Stage LinkedStage { get; set; }
        public CellDoor LinkedDoor { get; set; }

        public CellDoor(int x, int y, DoorKind doorKind)
            : base(GetCellKind(doorKind), x, y, true)
        {
            DoorKind = doorKind;
            CanBuildRoad = false;
        }
        
        private static CellKind GetCellKind(DoorKind doorKind)
        {
            switch(doorKind)
            {
                case DoorKind.UpDown:
                case DoorKind.DownUp:
                    return CellKind.Stair;
                default:
                    return CellKind.Door;
            }
        }
    }

    public enum DoorKind
    {
        LeftRight,
        RightLeft,
        TopBottom,
        BottomTop,
        UpDown,
        DownUp
    }
}
