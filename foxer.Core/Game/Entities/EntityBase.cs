using foxer.Core.Game.Animation;
using foxer.Core.Game.Entities.Descriptors;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace foxer.Core.Game.Entities
{
    public abstract class EntityBase
    {
        private bool _destroying = false;
        private Coroutine<EntityAnimation, EntityCoroutineArgs> _coroutine = new Coroutine<EntityAnimation, EntityCoroutineArgs>();

        public int CellX { get; private set; }

        public int CellY { get; private set; }

        public Point Cell => new Point(CellX, CellY);

        public Point PreviousFrameCell { get; private set; }

        public float X { get; private set; }

        public float Y { get; private set; }

        public float Z { get; private set; }

        public PointF Location => new PointF((float)X, (float)Y);

        public int Rotation { get; set; }

        public float HitPoints { get; set; }

        public EntityAnimation ActiveAnimation => _coroutine.Current;

        private EntityAnimation _destroyAnimation;

        public EntityAnimation Destroy => _destroyAnimation == null 
            ? _destroyAnimation = GetDestroyAnimation() 
            : _destroyAnimation;

        protected EntityBase(int x, int y, float z)
        {
            X = x;
            Y = y;
            Z = z;
            CellX = x;
            CellY = y;
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

            CellX = cellX;
            CellY = cellY;
            X = x;
            Y = y;
            return true;
        }

        public bool Teleport(Stage stage, float x, float y, IPlatform platform)
        {
            CellX = (int)Math.Round(x);
            CellY = (int)Math.Round(y);
            X = x;
            Y = y;
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
