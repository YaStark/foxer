namespace foxer.Core.Game.Generator.StageGenerator
{
    public class StagePlayerGenerator : StageGeneratorBase
    {
        public override void Generate(Stage stage, StageGeneratorArgs args)
        {
            if (args.PlayerSpawnLocation != null)
            {
                stage.ActiveEntity.X = args.PlayerSpawnLocation.Value.X;
                stage.ActiveEntity.Y = args.PlayerSpawnLocation.Value.Y;
                stage.ActiveEntity.CellX = args.PlayerSpawnLocation.Value.X;
                stage.ActiveEntity.CellY = args.PlayerSpawnLocation.Value.Y;
                stage.TryCreateNow(stage.ActiveEntity);
            }
        }
    }
}
