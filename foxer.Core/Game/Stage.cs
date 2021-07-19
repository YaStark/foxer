using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Generator.StageGenerator;
using foxer.Core.Game.Items;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game
{
    public class Stage
    {
        private const double MAX_TREES_ON_LVL_RATE = 0.3;

        private static readonly Random _rnd = new Random();
        private readonly List<StageGeneratorBase> _generators = new List<StageGeneratorBase>();
        private readonly List<EntityBase>[,] _entitesToCells;
        private readonly List<EntityBase> _entitesToRemove = new List<EntityBase>();
        private readonly List<EntityBase> _entitesToAdd = new List<EntityBase>();
        private readonly Game _game;

        public IList<EntityBase> Entities { get; private set; }

        public LootManager LootManager { get; } = new LootManager();

        public EntityPathManager PathManager { get; }

        public StressManager StressManager { get; }

        public ItemManager ItemManager => _game.ItemManager;

        public CellBase[,] Cells { get; private set; }

        public int Width { get; }

        public int Height { get; }

        public PlayerEntity ActiveEntity { get; set; }

        public string StageName { get; }

        public InventoryManager InventoryManager => _game.InventoryManager;

        public Stage(Game game, string name, int width, int height)
        {
            _game = game;
            Entities = new List<EntityBase>();
            PathManager = new EntityPathManager(this);
            StressManager = new StressManager();
            StageName = name;
            Width = width;
            Height = height;

            _entitesToCells = new List<EntityBase>[Width, Height];
            for (int i = 0; i < Width; i++) for (int j = 0; j < Height; j++) _entitesToCells[i, j] = new List<EntityBase>();

            _generators.Add(new StageBoundsGenerator());
            _generators.Add(new StageFloorGenerator());
            _generators.Add(new StageTreeGenerator());
            _generators.Add(new StageRoadGenerator());
            // _generators.Add(new LandfallGenerator());
            _generators.Add(new StageWaterGenerator(width));
            _generators.Add(new StageRocksGenerator());
            _generators.Add(new StagePlayerGenerator());
            _generators.Add(new StageNatureGenerator());
        }

        public void Generate(StageGeneratorArgs args)
        {
            Cells = new CellBase[Width, Height];
            foreach(var generator in _generators)
            {
                generator.Generate(this, args);
            }
        }

        public void AfterUpdate()
        {
            foreach(var entity in _entitesToRemove)
            {
                Entities.Remove(entity);
                _entitesToCells[entity.CellX, entity.CellY].Remove(entity);
            }

            _entitesToRemove.Clear();

            foreach(var entity in Entities.Where(e => e.Cell != e.PreviousFrameCell))
            {
                _entitesToCells[entity.PreviousFrameCell.X, entity.PreviousFrameCell.Y].Remove(entity);
                _entitesToCells[entity.CellX, entity.CellY].Add(entity);
            }

            foreach (var entity in _entitesToAdd)
            {
                TryCreateNow(entity);
            }

            _entitesToAdd.Clear();

            StressManager.Update(this);
        }

        public CellBase GetCell(int x, int y)
        {
            return x < 0
                || Cells.GetLength(0) <= x
                || y < 0
                || Cells.GetLength(1) <= y
                ? null
                : Cells[x, y];
        }

        public bool CheckArrayBounds(int i, int j)
        {
            return i >= 0 && i < Width && j >= 0 && j < Height;
        }

        public bool CheckCanWalkOnCell(EntityBase walker, int x, int y)
        {
            if (!CheckArrayBounds(x, y))
            {
                return false;
            }

            return PathManager.CanWalkBy(walker, x, y);
        }

        internal void LoadLevel(CellDoor door)
        {
            _game.LoadLevel(door.LinkedStage, door.LinkedDoor.X, door.LinkedDoor.Y);
        }

        public IEnumerable<EntityBase> GetEntitesInCell(int x, int y)
        {
            return CheckArrayBounds(x, y) 
                ? _entitesToCells[x, y] 
                : Enumerable.Empty<EntityBase>();
        }

        public IEnumerable<TEntity> GetNearestEntitesByType<TEntity>(int x, int y, int count)
        {
            int x0 = x;
            int x1 = x;
            int y0 = y;
            int y1 = y;
            List<TEntity> items = new List<TEntity>();
            while (items.Count < count)
            {
                for (int xi = x0; xi <= x1; xi++)
                {
                    items.AddRange(_entitesToCells[xi, y0].OfType<TEntity>());
                    if (y1 != y0)
                    {
                        items.AddRange(_entitesToCells[xi, y1].OfType<TEntity>());
                    }
                }

                for (int yj = y0 + 1; yj <= y1 - 1; yj++)
                {
                    items.AddRange(_entitesToCells[x0, yj].OfType<TEntity>());
                    if (x1 != x0)
                    {
                        items.AddRange(_entitesToCells[x1, yj].OfType<TEntity>());
                    }
                }

                x0 = Math.Max(0, x0 - 1);
                y0 = Math.Max(0, y0 - 1);
                x1 = Math.Min(Width - 1, x1 + 1);
                y1 = Math.Min(Height - 1, y1 + 1);

                if (x0 == 0 && y0 == 0 && x1 == Width - 1 && y1 == Height - 1)
                {
                    break;
                }
            }

            return items;
        }

        public void RemoveEntity(EntityBase entity)
        {
            _entitesToRemove.Add(entity);
        }

        public void TryGrowTree(Point pt)
        {
            var cell = GetCell(pt.X, pt.Y);
            double prob = Entities.OfType<TreeEntity>().Count() / MAX_TREES_ON_LVL_RATE / Width / Height;
            if (cell != null && _rnd.NextDouble() < prob)
            {
                _entitesToAdd.Add(new TreeEntity(cell, _rnd.Next()));
            }
        }

        public void AddEntity(EntityBase entity)
        {
            if (entity != null)
            {
                _entitesToAdd.Add(entity);
            }
        }

        internal bool TryCreateNow(EntityBase entity)
        {
            if(entity.CanBeCreated(this, entity.CellX, entity.CellY))
            {
                Entities.Add(entity);
                _entitesToCells[entity.CellX, entity.CellY].Add(entity);
                return true;
            }

            return false;
        }
    }
}
