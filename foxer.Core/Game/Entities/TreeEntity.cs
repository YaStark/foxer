using foxer.Core.Game.Animation;
using foxer.Core.Game.Cells;

namespace foxer.Core.Game.Entities
{
    public class TreeEntity : EntityBase
    {
        private const int MS_TO_INCREASE_AGE = 1000;
        private const int AGEMS_MAX = (AGE_LARGE + 5) * MS_TO_INCREASE_AGE;

        public const int AGE_MEDIUM = 120;
        public const int AGE_LARGE = 600;

        private uint _ageMs;

        public int Kind { get; }

        public uint Age => _ageMs / MS_TO_INCREASE_AGE;

        public SimpleAnimation Shake { get; }

        public SquirrelEntity Guest { get; set; }

        public TreeEntity(CellBase cell, int kind, uint age = 0)
            : base(cell.X, cell.Y)
        {
            _ageMs = age * MS_TO_INCREASE_AGE;
            Kind = kind;
            Shake = new SimpleAnimation(400);
        }

        protected override void OnUpdate(Stage stage, uint timeMs)
        {
            if(_ageMs < AGEMS_MAX)
            {
                _ageMs += timeMs;
            }
        }

        public void DoShake()
        {
            Guest?.RunAway();
            StartAnimation(Shake.Coroutine);
        }
    }
}
