using foxer.Core.Game.Animation;
using foxer.Core.Game.Cells;

namespace foxer.Core.Game.Entities
{
    public class BubblesEntity : EntityBase
    {
        public SimpleAnimation Bubble { get; }

        public WaitingAnimation Pause { get; }

        public BubblesEntity(CellBase cell)
            : base(cell.X, cell.Y)
        {
            Bubble = new SimpleAnimation(1000);
            Pause = new WaitingAnimation(500, 5000);
        }

        protected override void OnUpdate(Stage stage, uint timeMs)
        {
            if(ActiveAnimation == null)
            {
                StartAnimation(Pause.Coroutine, Bubble.Coroutine);
            }
        }
    }
}
