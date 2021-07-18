namespace foxer.Render.Menu
{
    public class GridCellInfo
    {
        public int ColumnSpan { get; }
        public int RowSpan { get; }
        public IMenuItem Item { get; }

        public GridCellInfo(int columnSpan, int rowSpan, IMenuItem item)
        {
            ColumnSpan = columnSpan;
            RowSpan = rowSpan;
            Item = item;
        }
    }
}
