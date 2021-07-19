using foxer.Core.Game;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;
using System.Drawing;

namespace foxer.Core.ViewModel.Menu
{
    public class GameUIViewModel : GameMenuViewModelBase, IInventoryManager
    {
        public IItemHolder[] FastPanel { get; private set; }

        public IInventoryManager InventoryManager => this;

        public ItemBase Hand => ViewModel.ActiveEntity?.Hand;

        public PlayerEntity ActiveEntity => ViewModel.ActiveEntity;

        public GameUIViewModel(PageGameViewModel viewModel)
            : base(viewModel)
        {
            FastPanel = new IItemHolder[ViewModel.FastPanelSize];
            for (int i = 0; i < FastPanel.Length; i++)
            {
                FastPanel[i] = new FastPanelItemHolder(viewModel, i);
            }
        }

        public void SetSelected(IItemHolder itemHolder)
        {
            for (int i = 0; i < FastPanel.Length; i++)
            {
                if(FastPanel[i] == itemHolder)
                {
                    ViewModel.FastPanelSelectedIndex = i;
                    break;
                }
            }

            if(ViewModel.ActiveEntity != null)
            {
                ViewModel.SetActiveItem(itemHolder.Get());
            }
        }

        public bool GetSelected(IItemHolder itemHolder)
        {
            return false;
        }

        public bool ProcessClickOnBuildableLayer(float x, float y)
        {
            if (Stage == null)
            {
                return false;
            }

            var touchedCell = new Point((int)x, (int)y);
            if (Stage.ActiveEntity?.Hand is IBuildableItem buildable
                && buildable.CheckBuildDistance(Stage.ActiveEntity.Cell, touchedCell))
            {
                if (buildable.CheckBuildDistance(Stage.ActiveEntity.Cell, touchedCell)
                    && buildable.CheckCanBuild(Stage, touchedCell.X, touchedCell.Y))
                {
                    Stage.AddEntity(buildable.Create(Stage, touchedCell.X, touchedCell.Y));
                    Stage.InventoryManager.Remove(Stage.ActiveEntity.Hand);
                    Stage.ActiveEntity.Hand = null;
                }

                return true;
            }

            return false;
        }

        public bool GetActive(IItemHolder itemHolder)
        {
            return ViewModel.FastPanelSelectedIndex >= 0
                && ViewModel.FastPanelSelectedIndex < FastPanel.Length
                && FastPanel[ViewModel.FastPanelSelectedIndex] == itemHolder;
        }
    }
}
