using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game.Interactors
{
    public abstract class InteractorBase<TSubject, TObject> : IInteractor
    {
        public bool CanInteractWith(object subj, object obj, InteractorArgs arg)
        {
            return subj is TSubject tsubj
                && obj is TObject tobj
                && CanInteract(tsubj, tobj, arg);
        }

        public bool InteractWith(object subj, object obj, InteractorArgs arg)
        {
            return Interact((TSubject)subj, (TObject)obj, arg);
        }

        protected abstract bool CanInteract(TSubject subj, TObject obj, InteractorArgs arg);
        protected abstract bool Interact(TSubject subj, TObject obj, InteractorArgs arg);

        protected static Point[] GetPathToItem(EntityBase walker, EntityBase target, Stage stage)
        {
            // todo set distance
            var pts = target.Cell.Nearest4()
                .Where(pt => stage.CheckCanWalkOnCell(walker, pt.X, pt.Y))
                .ToArray();

            if (!pts.Any())
            {
                return null;
            }

            return new WalkWithOptionTargetsBuilder(stage, walker, null, null, pts).ShortestPath;
        }
    }
}
