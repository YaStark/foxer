namespace foxer.Core.Game.Generator.StageGenerator
{
    public class StagePlayerGenerator : StageGeneratorBase
    {
        public override void Generate(Stage stage, StageGeneratorArgs args)
        {
            if (args.PlayerSpawnLocation != null)
            {
                stage.ActiveEntity.Teleport(
                    stage, 
                    args.PlayerSpawnLocation.Value.X, 
                    args.PlayerSpawnLocation.Value.Y, 
                    stage.DefaultPlatform);
                stage.TryCreateNow(stage.ActiveEntity);
            }
        }
    }
}
