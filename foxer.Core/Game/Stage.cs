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
        private readonly List<IPlatform>[,] _platforms;
        private readonly Dictionary<Type, List<EntityBase>> _entitesByType = new Dictionary<Type, List<EntityBase>>();

        private readonly List<EntityBase> _entitesToRemove = new List<EntityBase>();
        private readonly List<EntityBase> _entitesToAdd = new List<EntityBase>();
        private readonly List<EntityBase> _entites = new List<EntityBase>();
        private readonly List<IWall> _walls = new List<IWall>();

        public WalkBuilderFieldFactory WalkBuilderFieldFactory { get; }

        public IPlatform DefaultPlatform { get; } = new DefaultStagePlatform();

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
            WalkBuilderFieldFactory = new WalkBuilderFieldFactory(width, height);
            StageName = name;
            Width = width;
            Height = height;

            _entitesToCells = new List<EntityBase>[Width, Height];
            _platforms = new List<IPlatform>[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    _entitesToCells[i, j] = new List<EntityBase>();
                    _platforms[i, j] = new List<IPlatform>();
                    _platforms[i, j].Add(DefaultPlatform);
                }
            }

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

        public IPlatform GetLowerPlatform(EntityBase entity, int x, int y, float z)
        {
            var list = _platforms[x, y];
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if(!list[i].Active(this) 
                    || !list[i].CanSupport(entity))
                {
                    continue;
                }

                if (list[i].Level <= z)
                {
                    return list[i];
                }
            }

            return DefaultPlatform;
        }

        public IPlatform GetTopPlatform(EntityBase entity, int x, int y)
        {
            return _platforms[x, y].Where(p => p.CanSupport(entity)).Last();
        }

        public IPlatform GetPlatform(EntityBase target)
        {
            return GetLowerPlatform(target, target.CellX, target.CellY, target.Z + 0.01f);
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

                if (entity is IWall wall)
                {
                    _walls.Remove(wall);
                }

                if(entity is IPlatform platform)
                {
                    _platforms[entity.CellX, entity.CellY].Remove(platform);
                }
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
            WalkBuilderFieldFactory.Update(this);
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

        public IPlatform GetPlatformOnTransit(EntityBase walker, Point from, Point to, IPlatform platformFrom)
        {
            if (!CheckArrayBounds(from.X, from.Y)
                || IsWallBetweeen(from, to))
            {
                return null;
            }
            
            // ensure can climb
            var maxClimbHeight = walker.GetMaxClimbHeight(this);
            for (int i = 0; i < _platforms[to.X, to.Y].Count; i++)
            {
                var platform = _platforms[to.X, to.Y][i];
                if (platform.CanSupport(walker)
                    && platform.Active(this)
                    && Math.Abs(platform.Level - platformFrom.Level) <= maxClimbHeight
                    && CanBePlaced(walker, to.X, to.Y, platform))
                {
                    return platform;
                }
            }
            
            return null;
        }

        public bool CheckRoomZOnPlatform(EntityBase entity, int x, int y, IPlatform platform)
        {
            for (int i = _platforms[x, y].IndexOf(platform) + 1; i < _platforms[x, y].Count; i++)
            {
                if(!_platforms[x,y][i].IsColliderFor(entity.GetType()))
                {
                    continue;
                }

                return _platforms[x, y][i].Z - platform.Level > entity.GetHeight();
            }

            return true;
        }

        public bool CheckCanWalkToCell(EntityBase walker, Point to)
        {
            return null != GetPlatformOnTransit(
                walker, 
                walker.Cell, 
                to,
                GetPlatform(walker));
        }

        public bool IsWallBetweeen(Point a, Point b)
        {
            // todo platforms
            return _walls.Any(w => w.Active(this) && (w.Cell == a) && w.GetTransitPreventionTarget() == b)
                || _walls.Any(w => w.Active(this) && (w.Cell == b) && w.GetTransitPreventionTarget() == a);
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

        public IEnumerable<EntityBase> GetEntitesOnPlatform(int x, int y, IPlatform platform)
        {
            var i = _platforms[x, y].IndexOf(platform);
            if(i == _platforms[x, y].Count - 1)
            {
                return _entitesToCells[x, y].Where(e => e != platform && e.Z >= platform.Z);
            }
            else
            {
                var nextPlatformZ = _platforms[x, y][i + 1].Z;
                return _entitesToCells[x, y].Where(e => e != platform && e.Z >= platform.Z && e.Z < nextPlatformZ);
            }
        }

        public IEnumerable<TEntity> GetNearestEntitesByType<TEntity>(int x, int y, int count)
        {
            var entites = GetEntitesByType(typeof(TEntity));
            if(entites.Any())
            {
                var pt = new PointF(x + 0.5f, y + 0.5f);
                if (entites.Count() <= count) return entites.OrderBy(e => MathUtils.L2(pt, e.Cell)).Cast<TEntity>();
                return entites.OrderBy(e => MathUtils.L2(pt, e.Cell)).Take(count).Cast<TEntity>();
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
            if (!entity.CanBeCreated(this, entity.CellX, entity.CellY, GetPlatform(entity)))
            {
                return false;
            }

            _entites.Add(entity);
            _entitesToCells[entity.CellX, entity.CellY].Add(entity);

            var type = entity.GetType();
            if (!_entitesByType.ContainsKey(type))
            {
                _entitesByType[type] = new List<EntityBase>();
            }

            _entitesByType[type].Add(entity);

            if (entity is IWall wall)
            {
                _walls.Add(wall);
            }

            if (entity is IPlatform platform)
            {
                if (!_platforms[entity.CellX, entity.CellY].Any(p => p.Z > platform.Z))
                {
                    _platforms[entity.CellX, entity.CellY].Add(platform);
                }

                for (int i = 0; i < _platforms[entity.CellX, entity.CellY].Count; i++)
                {
                    if(_platforms[entity.CellX, entity.CellY][i].Z > platform.Z)
                    {
                        _platforms[entity.CellX, entity.CellY].Insert(i, platform);
                    }
                }
            }

            return true;
        }

        public IEnumerable<EntityBase> GetEntitesByType(Type entityType)
        {
            return _entitesByType.TryGetValue(entityType, out var result) 
                ? result 
                : Enumerable.Empty<EntityBase>();
        }

        public bool CanBePlaced(EntityBase entity, int x, int y, IPlatform platform)
        {
            return _game.GetDescriptor(entity.GetType()).CanBePlaced(this, entity, x, y, platform);
        }

        public ItemBase GetLoot(EntityBase entity)
        {
            return _game.GetDescriptor(entity.GetType()).GetLoot(this, entity);
        }
    }
}
