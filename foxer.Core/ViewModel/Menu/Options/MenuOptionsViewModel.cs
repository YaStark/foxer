using foxer.Core.Interfaces;
using System.Windows.Input;

namespace foxer.Core.ViewModel.Menu
{
    public class MenuOptionsViewModel : GameMenuViewModelBase
    {
        private const float ZOOM = 1.2f;
        private const float ZOOM_MIN = 0.7f;
        private const float ZOOM_MAX = 1 / ZOOM_MIN;

        private readonly DelegateCommand _commandZoomIn;
        private readonly DelegateCommand _commandZoomOut;
        private readonly DelegateCommand _commandExit;

        public ICommand CommandZoomIn => _commandZoomIn;
        public ICommand CommandZoomOut => _commandZoomOut;
        public ICommand CommandExit => _commandExit;

        public MenuOptionsViewModel(PageGameViewModel viewModel, INavigator navigator)
            : base(viewModel)
        {
            _commandZoomIn = new DelegateCommand(() => OnZoom(ZOOM), true);
            _commandZoomOut = new DelegateCommand(() => OnZoom(1 / ZOOM), true);
            _commandExit = new DelegateCommand(() => navigator.ShowMainPage(), true);
        }
        
        private void OnZoom(float zoom)
        {
            var scale = ViewModel.Scale * zoom;
            _commandZoomIn.Enabled = scale < ZOOM_MAX;
            _commandZoomOut.Enabled = scale > ZOOM_MIN;
            ViewModel.Scale = scale;
        }
    }
}
