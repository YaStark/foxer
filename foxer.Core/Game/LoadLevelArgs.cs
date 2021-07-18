namespace foxer.Core.Game
{
    public class LoadLevelArgs
    {
        public Stage Stage { get; }
        public int X { get; }
        public int Y { get; }

        public LoadLevelArgs(Stage stage, int x, int y)
        {
            Stage = stage;
            X = x;
            Y = y;
        }
    }
}
