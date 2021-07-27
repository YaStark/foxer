using foxer.Core.Game;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;
using System;
using System.Drawing;

namespace foxer.Core.ViewModel.Menu
{
    public class GameUIViewModel : GameMenuViewModelBase, IInventoryManager
    {
        public IInventoryManager InventoryManager => this;

        public ItemBase Hand => ViewModel.ActiveEntity?.Hand;

        public PlayerEntity ActiveEntity => ViewModel.ActiveEntity;

        public bool WalkMode => ViewModel.ActiveEntity.WalkMode;

        public GameUIViewModel(PageGameViewModel viewModel)
            : base(viewModel)
        {
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

            ViewModel.SetActiveItem(itemHolder.Get());
            ViewModel.ActiveEntity.WalkMode = false;
        }

        public bool ShowRotationPanel()
        {
            return ActiveEntity?.Hand is IBuildableItem item
                && item.UseRotation();
        }

        public void SetItemRotation(int angle)
        {
            var item = ActiveEntity?.Hand as IBuildableItem;
            item?.RotatePreview(angle);
        }

        public void SetWalkMode()
        {
            ViewModel.ActiveEntity.WalkMode = true;
            ViewModel.SetActiveItem(null);
        }

        public bool GetSelected(IItemHolder itemHolder)
        {
            return false;
        }

        public bool ProcessClickOnBuildableLayer(float x, float y, IPlatform platform)
        {
            if (Stage == null)
            {
                return false;
            }

            var touchedCell = new Point((int)x, (int)y);
            if(platform == null)
            {
                platform = Stage.DefaultPlatform;
            }

            if (Stage.ActiveEntity?.Hand is IBuildableItem buildable
                && buildable.CheckBuildDistance(Stage.ActiveEntity.Cell, touchedCell)) // todo check by Z too
            {
                if (buildable.CheckCanBuild(Stage, touchedCell.X, touchedCell.Y, platform.Level))
                {
                    Stage.AddEntity(buildable.Create(Stage, touchedCell.X, touchedCell.Y, platform.Level));

                    if(Stage.ItemManager.CanStack(Stage.ActiveEntity.Hand))
                    {
                        Stage.ActiveEntity.Hand.Count--;
                        if(Stage.ActiveEntity.Hand.Count == 0)
                        {
                            Stage.InventoryManager.Remove(Stage.ActiveEntity.Hand);
                            Stage.ActiveEntity.Hand = null;
                        }
                    }
                    else
                    {
                        Stage.InventoryManager.Remove(Stage.ActiveEntity.Hand);
                        Stage.ActiveEntity.Hand = null;
                    }
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
