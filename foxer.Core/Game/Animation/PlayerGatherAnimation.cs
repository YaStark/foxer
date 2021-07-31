using foxer.Core.Game.Entities;
using foxer.Core.Game.Items;
using foxer.Core.Utils;
using System.Collections.Generic;

namespace foxer.Core.Game.Animation
{
    public class PlayerGatherAnimation : EntityAnimation
    {
        private readonly SimpleAnimation _toolAnimation = new SimpleAnimation(1000);
        private readonly MoveToTargetAnimation _moveToTarget;
        private readonly PlayerEntity _host;

        public EntityBase Target
        {
            get { return _moveToTarget.Target; }
            set { _moveToTarget.Target = value; }
        }

        public PlayerGatherAnimation(PlayerEntity host) 
        {
            _host = host;
            _moveToTarget = new MoveToTargetAnimation(host, host.Walk);
        }
        
        public override IEnumerable<EntityAnimation> Coroutine(EntityCoroutineArgs args)
        {
            if (Target == null)
            {
                args.CancellationToken.Cancel();
                yield break;
            }

            _moveToTarget.MinDistance = GetTool().Distance;
            foreach (var item in _moveToTarget.Coroutine(args))
            {
                if(args.CancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                yield return item;
            }

            if (args.CancellationToken.IsCancellationRequested)
            {
                yield break;
            }

            var tool = GetTool();
            var swipes = tool.GetSwipesCount(Target);
            var hitMs = tool.HitMs;
            for (int i = 0; i < swipes; i++)
            {
                if (!OnToolSwipe())
                {
                    args.CancellationToken.Cancel();
                    yield break;
                }

                bool hit = false;
                uint time = 0;
                foreach (var item in _toolAnimation.Coroutine(args))
                {
                    if (args.CancellationToken.IsCancellationRequested)
                    {
                        yield break;
                    }

                    if (!hit)
                    {
                        time += args.DelayMs;
                        if (time > hitMs)
                        {
                            hit = true;
                            OnHit();
                        }
                    }

                    Progress = _toolAnimation.Progress;
                    yield return this;
                }
            }

            OnFinish(args);
        }

        private IToolItem GetTool()
        {
            return _host.Hand as IToolItem
                ?? _host.EmptyHandTool;
        }

        private void OnHit()
        {
            _host.SetWorkAggression();
            if (Target is TreeEntity tree)
            {
                tree.DoShake();
            }
        }

        private bool OnToolSwipe()
        {
            var tool = GetTool();
            if (tool.CanInteract(Target) != true)
            {
                return false;
            }

            _toolAnimation.DurationMs = tool.SwipeMs;
            return true;
        }

        private void OnFinish(EntityCoroutineArgs args)
        {
            if (Target is GrassEntity grass)
            {
                grass.Cut();
                args.Stage.CreateDroppedLootItem(grass);
            }
            else
            {
                Target.BeginDestroy();
            }

        }
    }
}
