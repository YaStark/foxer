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
    }
}
