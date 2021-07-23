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

        public int CellX { get; set; }

        public int CellY { get; set; }

        public Point Cell => new Point(CellX, CellY);

        public Point PreviousFrameCell { get; private set; }

        public double X { get; set; }

        public double Y { get; set; }

        public float Z { get; protected set; }

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

        public virtual bool CanBeCreated(Stage stage, int x, int y)
        {
            return stage.CanBePlaced(this, x, y);
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
