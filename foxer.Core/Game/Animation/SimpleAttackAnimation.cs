using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;

namespace foxer.Core.Game.Animation
{
    public class SimpleAttackAnimation : EntityAnimation
    {
        private readonly MoveToTargetAnimation _moveToTarget;
        private readonly SimpleAnimation _attack = new SimpleAnimation(1000);

        private EntityFighterBase _target;

        public EntityFighterBase Host { get; }

        public EntityFighterBase Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
                _moveToTarget.Target = value;
            }
        }

        public SimpleAttackAnimation(EntityFighterBase host, MovingByPathAnimation walk)
        {
            Host = host;
            _moveToTarget = new MoveToTargetAnimation(host, walk);
        }

        protected override IEnumerable<EntityAnimation> OnCoroutine(EntityCoroutineArgs args)
        {
            while(IsTargetAlive())
            {
                if (!CanAttackEnemy())
                {
                    args.CancellationToken.Cancel();
                    yield break;
                }

                _moveToTarget.MinDistance = Host.Weapon.Distance;
                // начинаем идти до цели, можем не дойти - цель спряталась
                foreach (var item in _moveToTarget.Coroutine(args))
                {
                    yield return item;
                }

                // начинаем бить, можем не убить - цель убежала
                foreach (var item in UseWeaponCoroutine(args))
                {
                    yield return item;
                }
            }
        }

        private IEnumerable<EntityAnimation> UseWeaponCoroutine(EntityCoroutineArgs args)
        {
            while(IsTargetAlive())
            {
                if (!BeforeHit(args))
                {
                    args.CancellationToken.Cancel();
                    yield break;
                }

                if(MathUtils.L1(Host.Cell, Target.Cell) > Host.Weapon.Distance)
                {
                    // цель отошла, надо опять к ней идти, да что такое-то
                    yield break;
                }

                bool hit = false;
                uint time = 0;
                foreach (var item in _attack.Coroutine(args))
                {
                    if (!hit)
                    {
                        time += args.DelayMs;
                        if (time > Host.Weapon?.HitMs)
                        {
                            hit = true;
                            OnHit(args);
                        }
                    }

                    Progress = _attack.Progress;
                    yield return this;
                }
            }

            yield break;
        }

        private bool IsTargetAlive()
        {
            return Target.Hitpoints > 0;
        }

        private void OnHit(EntityCoroutineArgs args)
        {
            Target.Hitpoints = Math.Max(0, Target.Hitpoints - Host.Weapon.GetDamage(args.Stage, Target));
            Target.SetAttacked(args.Stage, Host);
        }

        private bool BeforeHit(EntityCoroutineArgs args)
        { 
            if (!CanAttackEnemy())
            {
                return false;
            }

            _attack.DurationMs = Host.Weapon.SwipeMs;
            return true;
        }

        private bool CanAttackEnemy()
        {
            return Target != null
                && Host.Weapon?.CanInteract(Target) == true;
        }
    }
}
