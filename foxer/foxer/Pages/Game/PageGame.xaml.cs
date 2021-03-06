using foxer.Core.ViewModel;
using foxer.Core.Utils;
using foxer.Core.ViewModel.Menu;
using foxer.Pages.Game.Menu;
using foxer.Render;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace foxer.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageGame : ContentPage, ISingletoneFactory<IMenuHost>
    {
        public static bool EnableDebugGraphics = false;
        private readonly PageGameViewModel _vm;
        private readonly GameRenderer _renderer;
        private readonly INativeView _view;

        private DateTime _time;

        private readonly MenuHost _menuHost;

        IMenuHost ISingletoneFactory<IMenuHost>.Item => _menuHost;

        public PageGame(INavigator navigator, INativeViewFactory nativeCanvasFactory)
		{
			InitializeComponent();
            _vm = new PageGameViewModel(navigator, this);
            _menuHost = new MenuHost(_vm);
            _renderer = new GameRenderer(_vm, _menuHost);

            _view = nativeCanvasFactory.CreateView(_renderer);
            Content = _view.View;
            
            _vm.LoadGame();
            Device.StartTimer(TimeSpan.FromMilliseconds(25), OnTimerTick);
        }

        private bool OnTimerTick()
        {
            var time = DateTime.Now;
            var delayMs = (int)(time - _time).TotalMilliseconds;
            if(delayMs > 0)
            {
                _vm.Update((uint)delayMs);
                _menuHost.Menu.Update(delayMs);
                Device.BeginInvokeOnMainThread(() => OnUpdate(delayMs));
            }

            _time = time;
            return true;
        }

        private void OnUpdate(int delayMs)
        {
            _view.RequestRedraw();
        }
    }
}