using foxer.Core.Game.Entities;

namespace foxer.Core.Game.Generator.StageGenerator
{
    public class StagePlayerGenerator : StageGeneratorBase
    {
        public override void Generate(Stage stage, StageGeneratorArgs args)
        {
            if (args.PlayerSpawnLocation != null)
            {
                var player = new PlayerEntity(
                    args.PlayerSpawnLocation.Value.X,
                    args.PlayerSpawnLocation.Value.Y);
                stage.TryCreateNow(player);
                stage.ActiveEntity = player;
            }
        }
    }
}
