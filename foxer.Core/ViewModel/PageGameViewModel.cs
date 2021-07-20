using foxer.Core.Game;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Craft;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;
using foxer.Core.Interfaces;
using foxer.Core.Utils;
using foxer.Core.ViewModel.Menu;
using foxer.Core.ViewModel.Menu.Craft;
using System.Drawing;
using System.Windows.Input;

namespace foxer.Core.ViewModel
{
    public class PageGameViewModel : ViewModelBase
    {
        private readonly DelegateCommand _commandCloseMenu;
        private readonly DelegateCommand _commandOptions;
        private readonly DelegateCommand _commandInventory;
        private readonly DelegateCommand _commandCraft;

        public ICommand CommandCloseMenu => _commandCloseMenu;
        public ICommand CommandOptions => _commandOptions;
        public ICommand CommandInventory => _commandInventory;
        public ICommand CommandCraft => _commandCraft;

        private readonly Game.Game _game = new Game.Game();
        private int _fastPanelSelectedIndex = 0;

        public PlayerEntity ActiveEntity => _game.ActiveEntity;

        private readonly ISingletoneFactory<IMenuHost> _menuHostFactory;

        public Stage Stage => _game.Stage;

        public Size InventorySize => _game.InventorySize;

        public int FastPanelSize => _game.FastPanelSize;

        public PlayerHandsCrafter PlayerHandsCrafter => _game.PlayerHandsCrafter;

        public int FastPanelSelectedIndex
        {
            get
            {
                return ActiveEntity.WalkMode ? -1 : _fastPanelSelectedIndex;
            }
            set
            {
                _fastPanelSelectedIndex = value;
            }
        }

        public float Scale { get; set; } = 1;

        public MenuOptionsViewModel MenuOptions { get; }

        public MenuInventoryViewModel MenuInventory { get; }

        public GameUIViewModel GameUI { get; }

        public MenuCraftViewModel MenuCraft { get; }

        public IMenuHost GameMenu => _menuHostFactory.Item;

        public ItemManager ItemManager => _game.ItemManager;

        public PageGameViewModel(INavigator navigator, ISingletoneFactory<IMenuHost> menuHostFactory)
        {
            _menuHostFactory = menuHostFactory;

            MenuOptions = new MenuOptionsViewModel(this, navigator);
            MenuInventory = new MenuInventoryViewModel(this);
            GameUI = new GameUIViewModel(this);
            MenuCraft = new MenuCraftViewModel(this);

            _commandCloseMenu = new DelegateCommand(() => GameMenu.CloseMenu(), true);
            _commandOptions = new DelegateCommand(() => GameMenu.OpenOptionsMenu(), true);
            _commandInventory = new DelegateCommand(() => GameMenu.OpenInventoryMenu(), true);
            _commandCraft = new DelegateCommand(() => GameMenu.OpenCraftMenu(), true);
        }

        internal void SetActiveItem(ItemBase itemBase)
        {
            ActiveEntity.Hand = itemBase;
        }

        public void LoadGame()
        {
            _game.GenerateMap();
        }

        public CellBase[,] GetStageField()
        {
            return _game.Stage.Cells;
        }

        public RectangleF GetViewPort(SizeF canvasSize)
        {
            const int DEFAULT_SIZE = 6;
            float side1 = DEFAULT_SIZE / Scale;
            if(canvasSize.Width < canvasSize.Height)
            {
                float side2 = side1 * canvasSize.Height / canvasSize.Width;
                return new RectangleF(
                    (float)_game.Stage.ActiveEntity.X - side1 / 2,
                    (float)_game.Stage.ActiveEntity.Y - side2 / 2,
                    side1,
                    side2);
            }
            else
            {
                float side2 = side1 * canvasSize.Width / canvasSize.Height;
                return new RectangleF(
                    (float)_game.Stage.ActiveEntity.X - side2 / 2,
                    (float)_game.Stage.ActiveEntity.Y - side1 / 2,
                    side2,
                    side1);
            }
        }

        public bool ProcessClickOnEntityLayer(float x, float y)
        {
            ActiveEntity?.SetWalkTarget(new Point((int)x, (int)y));
            return true;
        }

        public void Update(uint delayMs)
        {
            _game.Update(delayMs);
        }
    }
}
