using foxer.Core.Game;
using foxer.Core.Game.Cells;
using foxer.Core.Game.Craft;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Info;
using foxer.Core.Game.Items;
using foxer.Core.Interfaces;
using foxer.Core.Utils;
using foxer.Core.ViewModel.Menu;
using foxer.Core.ViewModel.Menu.Craft;
using System.Drawing;

namespace foxer.Core.ViewModel
{
    public class PageGameViewModel : ViewModelBase
    {
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

        public ItemInfoManager ItemInfoManager { get; } = new ItemInfoManager();

        public PageGameViewModel(INavigator navigator, ISingletoneFactory<IMenuHost> menuHostFactory)
        {
            _menuHostFactory = menuHostFactory;

            MenuOptions = new MenuOptionsViewModel(this, navigator);
            MenuInventory = new MenuInventoryViewModel(this);
            GameUI = new GameUIViewModel(this);
            MenuCraft = new MenuCraftViewModel(this);
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
                    _game.Stage.ActiveEntity.X - side1 / 2,
                    _game.Stage.ActiveEntity.Y - side2 / 2,
                    side1,
                    side2);
            }
            else
            {
                float side2 = side1 * canvasSize.Width / canvasSize.Height;
                return new RectangleF(
                    _game.Stage.ActiveEntity.X - side2 / 2,
                    _game.Stage.ActiveEntity.Y - side1 / 2,
                    side2,
                    side1);
            }
        }

        public bool ProcessClickOnEntityLayer(int x, int y, EntityBase entity)
        {
            if(entity == null)
            {
                ActiveEntity.SetWalkTarget(Stage, x, y, Stage.DefaultPlatform);
                return true;
            }

            if (Stage == null
                || ActiveEntity.TryInteract(Stage, entity))
            {
                return true;
            }

            if(entity is IPlatform platform 
                && platform.CanSupport(ActiveEntity))
            {
                ActiveEntity.SetWalkTarget(Stage, x, y, platform);
                return true;
            }

            return false;
        }

        public void Update(uint delayMs)
        {
            _game.Update(delayMs);
        }
    }
}
