using foxer.Core.Game.Cells;

namespace foxer.Core.Game.Entities
{
    public class GrassEntity : EntityBase
    {
        private const uint MATURE_AGEMS = 60 * 1000;

        private uint _ageMs;

        public int Age => (int)_ageMs / 1000;

        public int Kind { get; }

        public bool CanGather => _ageMs >= MATURE_AGEMS;

        public GrassEntity(CellBase cell, int kind) 
            : base(cell.X, cell.Y, 0)
        {
            Kind = kind;
            _ageMs = MATURE_AGEMS;
        }

        protected override void OnUpdate(Stage stage, uint timeMs)
        {
            if(_ageMs < MATURE_AGEMS)
            {
                _ageMs += timeMs;
            }

            base.OnUpdate(stage, timeMs);
        }

        public void Cut()
        {
            _ageMs = 0;
        }

        public override bool UseHitbox()
        {
            return false;
        }
    }
}
