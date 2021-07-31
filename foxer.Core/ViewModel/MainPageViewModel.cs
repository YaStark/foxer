using foxer.Core.ViewModel;
using System.Windows.Input;

namespace foxer.Core
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INavigator _navigation;
        private string _greetingsText;
        private DelegateCommand _commandPlay;
        private string _title;

        public string Title
        {
            get => _title;
            set => SetValue(ref _title, value);
        }

        public string GreetingsText
        {
            get => _greetingsText;
            set => SetValue(ref _greetingsText, value);
        }

        public ICommand CommandPlay => _commandPlay;

        public MainPageViewModel(INavigator navigation)
        {
            _title = "Foxxer game";
            _navigation = navigation;
            _greetingsText = "Hello Player!";
            _commandPlay = new DelegateCommand(() => OnPlay(), true);
        }

        private void OnPlay()
        {
            _navigation.ShowGamePage();
        }
    }
}
