namespace foxer.Core.Game.Entities
{
    public abstract class EntityFighterBase : EntityBase
    {
        public virtual IWeaponItem Weapon { get; }

        public int Hitpoints { get; set; }

        public int MaxHitpoints { get; protected set; }
        
        protected EntityFighterBase(int x, int y, float z, int maxHitpoints) 
            : base(x, y, z)
        {
            MaxHitpoints = maxHitpoints;
            Hitpoints = maxHitpoints;
        }

        public virtual bool IsIdle()
        {
            return ActiveAnimation == null;
        }

        public void SetAttacked(Stage stage, EntityFighterBase aggressor)
        {
            OnAttacked(stage, aggressor);
        }

        protected virtual void OnAttacked(Stage stage, EntityFighterBase aggressor)
        {
            if (Hitpoints <= 0)
            {
                BeginDestroy();
            }
        }
    }
}
