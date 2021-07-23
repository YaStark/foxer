using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities;
using foxer.Core.Game.Entities.Descriptors;
using foxer.Core.Game.Entities.Stress;
using foxer.Core.Game.Generator.StageGenerator;
using foxer.Core.Game.Items;
using foxer.Core.Utils;
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
        private readonly Game _game;

        private readonly List<EntityBase>[,] _entitesToCells;
        private readonly Dictionary<Type, List<EntityBase>> _entitesByType = new Dictionary<Type, List<EntityBase>>();
        private readonly List<EntityBase> _entitesToRemove = new List<EntityBase>();
        private readonly List<EntityBase> _entitesToAdd = new List<EntityBase>();
        private readonly List<EntityBase> _entites = new List<EntityBase>();

        public Random Rnd { get; } = new Random();

        public IReadOnlyList<EntityBase> Entities => _entites;

        public StressManager StressManager { get; }

        public ItemManager ItemManager => _game.ItemManager;

        public CellBase[,] Cells { get; private set; }

        public int Width { get; }

        public int Height { get; }

        public PlayerEntity ActiveEntity => _game.ActiveEntity;

        public string StageName { get; }

        public InventoryManager InventoryManager => _game.InventoryManager;

        public Stage(Game game, string name, int width, int height)
        {
            _game = game;
            StressManager = new StressManager(_game.Descriptors);
            StageName = name;
            Width = width;
            Height = height;

            _entitesToCells = new List<EntityBase>[Width, Height];
            for (int i = 0; i < Width; i++) for (int j = 0; j < Height; j++) _entitesToCells[i, j] = new List<EntityBase>();

            _generators.Add(new StageBoundsGenerator());
            _generators.Add(new StageFloorGenerator());
            _generators.Add(new StageTreeGenerator());
            _generators.Add(new StageGrassGenerator());
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
            foreach (var generator in _generators)
            {
                generator.Generate(this, args);
            }
        }

        public void AfterUpdate()
        {
            foreach(var entity in _entitesToRemove)
            {
                _entites.Remove(entity);
                _entitesToCells[entity.CellX, entity.CellY].Remove(entity);
                _entitesByType[entity.GetType()].Remove(entity);
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

        public EntityDescriptorBase GetDescriptor(Type entityType)
        {
            return _game.GetDescriptor(entityType);
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

            return CanBePlaced(walker, x, y);
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
            var entites = GetEntitesByType(typeof(TEntity));
            if(entites.Any())
            {
                var pt = new Point(x, y);
                entites = entites.OrderBy(e => MathUtils.L1(pt, e.Cell));
                if (entites.Count() <= count) return entites.Cast<TEntity>();
                return entites.Take(count).Cast<TEntity>();
            }

            return Enumerable.Empty<TEntity>();
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
                _entites.Add(entity);
                _entitesToCells[entity.CellX, entity.CellY].Add(entity);
                var type = entity.GetType();
                if (!_entitesByType.ContainsKey(type))
                {
                    _entitesByType[type] = new List<EntityBase>();
                }
                _entitesByType[type].Add(entity);
                return true;
            }

            return false;
        }

        public IEnumerable<EntityBase> GetEntitesByType(Type entityType)
        {
            return _entitesByType.TryGetValue(entityType, out var result) 
                ? result 
                : Enumerable.Empty<EntityBase>();
        }

        public bool CanBePlaced(EntityBase entity, int x, int y)
        {
            return _game.GetDescriptor(entity.GetType()).CanBePlaced(this, entity, x, y, entity.Z);
        }

        public bool CanBeCreated(EntityBase entity, int x, int y)
        {
            return CanBePlaced(entity, x, y);
        }

        public bool CanBeCreated<T>(int x, int y, float z = 0)
        {
            return _game.GetDescriptor(typeof(T)).CanBePlaced(this, null, x, y, z);
        }
        
        public ItemBase GetLoot(EntityBase entity)
        {
            return _game.GetDescriptor(entity.GetType()).GetLoot(this, entity);
        }
    }
}
