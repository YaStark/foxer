using foxer.Core.Game.Attack;

namespace foxer.Core.Game.Entities
{
    public abstract class EntityFighterAIBase : EntityFighterBase
    {
        protected EntityFighterAIBase(int x, int y, float z, int maxHitpoints)
            : base(x, y, z, maxHitpoints)
        {
        }

        public abstract bool BeginFight(Stage stage, EntityFighterBase enemy);

        public abstract bool BeginRunaway(Stage stage, IAwareEntitesProvider awareEntitesProvider);

        public abstract void EndFight();
    }
}
