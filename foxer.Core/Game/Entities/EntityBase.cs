using foxer.Core.Game.Animation;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game.Entities
{
    public abstract class EntityBase
    {
        private EntityAnimation _destroyAnimation;

        private bool _destroying = false;

        private Coroutine<EntityAnimation, EntityCoroutineArgs> _coroutine = new Coroutine<EntityAnimation, EntityCoroutineArgs>();

        public int CellX => Cell.X;

        public int CellY => Cell.Y;

        public Point Cell { get; private set; } 

        public Point PreviousFrameCell { get; private set; }

        public float X => Location.X;

        public float Y => Location.Y;

        public float Z { get; private set; }

        public PointF Location { get; private set; }

        public int Rotation { get; set; }

        public EntityAnimation ActiveAnimation => _coroutine.Current;

        public EntityAnimation Destroy => _destroyAnimation == null 
            ? _destroyAnimation = GetDestroyAnimation() 
            : _destroyAnimation;

        protected EntityBase(int x, int y, float z)
        {
            Cell = new Point(x, y);
            Location = new PointF(x, y);
            Z = z;
        }

        public void Update(Stage stage, uint timeMs)
        {
            OnUpdate(stage, timeMs);

            if (ActiveAnimation == null && _destroying)
            {
                stage.RemoveEntity(this);
                OnDestroy(stage, timeMs);
                return;
            }

            PreviousFrameCell = Cell;
            _coroutine.Argument.DelayMs = timeMs;
            _coroutine.Argument.Stage = stage;
            _coroutine.Update();
        }

        public void RotateTo(Point cell)
        {
            Rotation = GeomUtils.GetAngle90(cell, Cell);
        }

        protected virtual void OnUpdate(Stage stage, uint timeMs)
        {
        }

        public virtual bool CanBeCreated(Stage stage, int x, int y, IPlatform platform)
        {
            return stage.CanBePlaced(this, x, y, platform);
        }
        
        public virtual bool UseHitbox()
        {
            return true;
        }

        public virtual float GetHeight()
        {
            return 1f;
        }

        /// <summary>
        /// Use to describe offset the real bounds from the current Location
        /// </summary>
        public virtual float GetZIndex()
        {
            return 0;
        }

        public virtual float GetMaxClimbHeight(Stage stage)
        {
            return 0.25f;
        }

        public void ClearAnimation()
        {
            _coroutine.Stop();
        }

        public void BeginDestroy()
        {
            _destroying = true;
            StartAnimation(Destroy.Coroutine);
        }

        public bool TryMoveXY(Stage stage, float x, float y)
        {
            int cellX = (int)Math.Round(x);
            int cellY = (int)Math.Round(y);
            if (CellX != cellX || CellY != cellY)
            {
                var prevPlatform = stage.GetLowerPlatform(this, CellX, CellY, Z + 0.01f);
                var newPlatform = stage.GetPlatformOnTransit(this, Cell, new Point(cellX, cellY), prevPlatform);
                if (newPlatform == null)
                {
                    return false;
                }

                Z = newPlatform.Level;
            }

            Cell = new Point(cellX, cellY);
            Location = new PointF(x, y);
            return true;
        }

        public bool Teleport(Stage stage, float x, float y, IPlatform platform)
        {
            Cell = new Point((int)Math.Round(x), (int)Math.Round(y));
            Location = new PointF(x, y);
            Z = platform.Level;
            return true;
        }

        public void MoveZ(float z)
        {
            Z = z;
        }

        protected virtual EntityAnimation GetDestroyAnimation()
        {
            return new SimpleAnimation(1);
        }

        protected virtual void OnDestroy(Stage stage, uint timeMs)
        {
            stage.CreateDroppedLootItem(this);
        }
        
        protected void StartAnimation(Func<EntityCoroutineArgs, IEnumerable<EntityAnimation>>[] coroutines)
        {
            _coroutine.Start(coroutines);
        }

        protected void StartAnimation(
            Func<EntityCoroutineArgs, IEnumerable<EntityAnimation>> first,
            params Func<EntityCoroutineArgs, IEnumerable<EntityAnimation>>[] coroutines)
        {
            StartAnimation(new[] { first }.Concat(coroutines).ToArray());
        }
    }
}
