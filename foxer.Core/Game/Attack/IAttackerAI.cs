using foxer.Core.Game.Entities;
using System.Collections.Generic;

namespace foxer.Core.Game.Attack
{
    public interface IAttackerAI
    {
        EntityFighterBase AttackTarget { get; }

        AttackerAIBehavior CurrentBehavior { get; }

        bool OnUpdate(Stage stage, EntityFighterAIBase host, uint delayMs);

        void OnAttacked(EntityFighterBase aggressor);
    }

    public interface IAwareEntitesProvider
    {
        IEnumerable<EntityBase> GetAwareEntites();
    }
}
