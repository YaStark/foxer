using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game.Interactors
{
    public abstract class InteractorBase<TSubject> : IInteractor
    {
        public bool CanInteractWith(object subj, object obj, InteractorArgs arg)
        {
            return subj is TSubject tsubj && CanInteract(tsubj, obj, arg);
        }

        public bool InteractWith(object subj, object obj, InteractorArgs arg)
        {
            return Interact((TSubject)subj, obj, arg);
        }

        protected abstract bool CanInteract(TSubject subj, object obj, InteractorArgs arg);
        protected abstract bool Interact(TSubject subj, object obj, InteractorArgs arg);

        protected static Point[] GetPathToItem(EntityBase walker, EntityBase target, Stage stage)
        {
            // todo set distance
            var targetPlatform = stage.GetPlatform(target);
            var cells = target.Cell.Nearest4()
                // transit from target cell to nearest4 cells
                .Select(pt => new WalkCell(pt, stage.GetPlatformOnTransit(walker, target.Cell, pt, targetPlatform)))
                .Where(cell => cell.Platform != null)
                .ToArray();

            if (!cells.Any())
            {
                return null;
            }

            using (var builder = new WalkBuilderWithMultipleTargets(stage, walker, null, null, cells))
            {
                return builder.GetShortestPath();
            }
        }
    }
}
