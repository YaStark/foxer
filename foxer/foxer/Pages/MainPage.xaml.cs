using foxer.Core;
using foxer.Core.ViewModel;
using Xamarin.Forms;

namespace foxer
{
    public partial class MainPage : ContentPage
    {
        public MainPage(INavigator nav)
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel(nav);
        }
    }
}
