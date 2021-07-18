namespace foxer.Core.Game.Interactors
{
    public interface IInteractor
    {
        bool CanInteractWith(object subj, object obj, InteractorArgs arg);
        bool InteractWith(object subj, object obj, InteractorArgs arg);
    }
}
