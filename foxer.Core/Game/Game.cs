using foxer.Core.Game.Cells;
using foxer.Core.Game.Craft;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Generator;
using foxer.Core.Game.Generator.StageGenerator;
using foxer.Core.Game.Items;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game
{
    public class Game
    {
        public const int MAX_FAST_PANEL_SIZE = 4;

        private readonly DoorGenerator _doorGenerator = new DoorGenerator();
        private LoadLevelArgs _loadLevelArgs;

        public Stage Stage { get; set; }

        public Size InventorySize { get; } = new Size(5, 4);

        public int FastPanelSize { get; private set; } = 2;

        public ItemBase[] Inventory { get; }

        public InventoryManager InventoryManager { get; }

        public ItemManager ItemManager { get; } = new ItemManager();

        public PlayerHandsCrafter PlayerHandsCrafter { get; }

        public static int Seed { get; private set; }

        public PlayerEntity ActiveEntity { get; internal set; }

        public Game()
        {
            Inventory = new ItemBase[InventorySize.Width * InventorySize.Height + MAX_FAST_PANEL_SIZE];
            Inventory[10] = ItemManager.Create<ItemStone>(null, 16);
            Inventory[11] = ItemManager.Create<ItemStoneOven>(null);

            InventoryManager = new InventoryManager(this);
            PlayerHandsCrafter = new PlayerHandsCrafter(this);
        }

        public void GenerateMap(int seed = 0)
        {
            if (seed == 0) seed = DateTime.Now.Millisecond;
            Seed = seed;
            Debug.WriteLine($"### Game generation is started. Seed: {Seed} ###");

            ActiveEntity = new PlayerEntity(0, 0);
            ActiveEntity.WalkMode = true;

            const int grid = 50;
            var rnd = new Random(seed == 0 ? DateTime.Now.Millisecond : seed);
            var stage0 = new Stage(this, "0;0;0", grid, grid);
            var stage1 = new Stage(this, "1;0;0", grid, grid);
            
            (var door12, var door21) = _doorGenerator.LinkStagesLeftRight(stage0, stage1, GetRndDoor(grid, rnd));
            stage0.Generate(new StageGeneratorArgs(rnd, door12)
            {
                PlayerSpawnLocation = new System.Drawing.Point(1, 1)
            });

            Stage = stage0;
            // map NxNxN

            var path = CreatePath(0, 0, 15, rnd);
            path.Reverse();
            path = path.Take(3).ToList();

            Stage prevStage = stage1;
            List<CellDoor> doorsPrev = new List<CellDoor>();
            doorsPrev.Add(door21);
            int i0 = 0;
            int j0 = 0;
            for (int k = 1; k < path.Count; k++)
            {
                List<CellDoor> doorsCur = new List<CellDoor>();
                (int i, int j) = path[k];
                var stage = new Stage(this, $"{i};{j}", grid, grid);

                if(j0 < j)
                {
                    var doors = _doorGenerator.LinkStagesTopBottom(prevStage, stage, GetRndDoor(grid, rnd));
                    doorsCur.Add(doors.Item2);
                    doorsPrev.Add(doors.Item1);
                }
                else if(j0 > j)
                {
                    var doors = _doorGenerator.LinkStagesTopBottom(stage, prevStage, GetRndDoor(grid, rnd));
                    doorsCur.Add(doors.Item1);
                    doorsPrev.Add(doors.Item2);
                }

                if (i0 < i)
                {
                    var doors = _doorGenerator.LinkStagesLeftRight(prevStage, stage, GetRndDoor(grid, rnd));
                    doorsCur.Add(doors.Item2);
                    doorsPrev.Add(doors.Item1);
                }
                else if (i0 > i)
                {
                    var doors = _doorGenerator.LinkStagesLeftRight(stage, prevStage, GetRndDoor(grid, rnd));
                    doorsCur.Add(doors.Item1);
                    doorsPrev.Add(doors.Item2);
                }

                prevStage.Generate(new StageGeneratorArgs(rnd, doorsPrev.ToArray()));
                doorsPrev = doorsCur;
                i0 = i;
                j0 = j;
                prevStage = stage;
            }

            prevStage.Generate(new StageGeneratorArgs(rnd, doorsPrev.ToArray()));
        }

        internal bool InteractWithCell(Point touchedCell)
        {
            if (Stage.ActiveEntity == null)
            {
                return true;
            }

            if(Stage.ActiveEntity?.Hand is IBuildableItem buildable
                && buildable.CheckBuildDistance(Stage.ActiveEntity.Cell, touchedCell))
            {
                if(buildable.CheckBuildDistance(Stage.ActiveEntity.Cell, touchedCell)
                    && buildable.CheckCanBuild(Stage, touchedCell.X, touchedCell.Y))
                {
                    Stage.AddEntity(buildable.Create(Stage, touchedCell.X, touchedCell.Y));
                    Stage.InventoryManager.Remove(Stage.ActiveEntity.Hand);
                    Stage.ActiveEntity.Hand = null;
                }

                return true;
            }

            Stage.ActiveEntity.SetWalkTarget(touchedCell);
            return true;
        }

        internal void LoadLevel(Stage stage, int x, int y)
        {
            _loadLevelArgs = new LoadLevelArgs(stage, x, y);
        }

        public void Update(uint delayMs)
        {
            var swh = new StopwatchHelper("PageGameViewModel.Update");
            swh.Scope("transit");
            if (_loadLevelArgs != null)
            {
                Transit();
                _loadLevelArgs = null;
            }

            swh.Scope("entities");
            foreach (var entity in Stage.Entities)
            {
                swh.Point($"{entity.GetType()}");
                entity.Update(Stage, delayMs);
            }

            swh.Scope("Stage.AfterUpdate");
            Stage.AfterUpdate();
            swh.Show();
        }

        private int GetRndDoor(int size, Random rnd)
        {
            var random = rnd.NextDouble() * rnd.NextDouble();
            return (int)(Math.Min(0.75, Math.Max(random, 0.25)) * size);
        }

        private void Transit()
        {
            var player = Stage.ActiveEntity;
            Stage.Entities.Remove(player);
            Stage = _loadLevelArgs.Stage;
            player.ClearAnimation();
            player.X = _loadLevelArgs.X;
            player.Y = _loadLevelArgs.Y;
            player.CellX = _loadLevelArgs.X;
            player.CellY = _loadLevelArgs.Y;
            _loadLevelArgs.Stage.TryCreateNow(player);
        }

        private List<(int X, int Y)> CreatePath(int startX, int startY, int requiredDistance, Random rnd)
        {
            var field = new int[requiredDistance, requiredDistance];
            field[0, requiredDistance / 2] = 1;

            var stack = new Stack<(int, int)>();
            stack.Push((0, requiredDistance / 2));
            while(stack.Any())
            {
                (int x, int y) = stack.Pop();
                int value = field[x, y] + 1;
                TryFillPathCell(x + 1, y, requiredDistance, field, stack, value + rnd.Next(2));
                TryFillPathCell(x - 1, y, requiredDistance, field, stack, value + rnd.Next(2));
                TryFillPathCell(x, y + 1, requiredDistance, field, stack, value + rnd.Next(2));
                TryFillPathCell(x, y - 1, requiredDistance, field, stack, value + rnd.Next(2));
            }

            int i = requiredDistance - 1;
            int j = requiredDistance / 2;
            List<(int X, int Y)> result = new List<(int X, int Y)>();
            while (field[i, j] != 1)
            {
                (i, j) = GetNextNearestMinimum(field, i, j, requiredDistance);
                result.Add((i + startX, j - requiredDistance / 2 + startY));
            }

            return result;
        }

        private (int, int) GetNextNearestMinimum(int[,] field, int i, int j, int size)
        {
            List<(int, int)> items = new List<(int, int)>();
            if (CheckBounds(i + 1, j, size)) items.Add((i + 1, j));
            if (CheckBounds(i, j - 1, size)) items.Add((i, j - 1));
            if (CheckBounds(i, j + 1, size)) items.Add((i, j + 1));
            if (CheckBounds(i - 1, j, size)) items.Add((i - 1, j));

            field[i, j] = int.MaxValue;
            return items.Where(x => field[x.Item1, x.Item2] != 0).OrderBy(x => field[x.Item1, x.Item2]).First();
        }

        private void TryFillPathCell(int i, int j, int size, int[,] field, Stack<(int, int)> stack, int value)
        {
            if (CheckBounds(i, j, size) && field[i, j] == 0)
            {
                field[i, j] = value;
                stack.Push((i, j));
            }
        }

        private bool CheckBounds(int x, int y, int size)
        {
            return x >= 0 && y >= 0 && x < size && y < size;
        }
    }
}
