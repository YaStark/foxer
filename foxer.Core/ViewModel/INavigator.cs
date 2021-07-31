namespace foxer.Core.ViewModel
{
    public interface INavigator
    {
        void ShowMainPage();
        void ShowGamePage();
        bool CanGoBack();
        void Back();
    }
}
