using foxer.Core.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace foxer.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageRoller : ContentPage
	{
		public PageRoller ()
		{
            InitializeComponent();
            BindingContext = new PageRollerViewModel();
        }
	}
}