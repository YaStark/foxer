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

        public EntityBase Host { get; }

        public EntityBase Target
        {
            get { return _moveToTarget.Target; }
            set { _moveToTarget.Target = value; }
        }

        public SimpleAttackAnimation(EntityBase host, MovingByPathAnimation walk)
        {
            Host = host;
            _moveToTarget = new MoveToTargetAnimation(host, walk);
        }

        public override IEnumerable<EntityAnimation> Coroutine(EntityCoroutineArgs args)
        {
            while(IsTargetAlive())
            {
                if (!CanAttackEnemy())
                {
                    args.CancellationToken.Cancel();
                    yield break;
                }

                _moveToTarget.MinDistance = Host.Attacker.Weapon.ToolDistance;
                // начинаем идти до цели, можем не дойти - цель спряталась
                foreach (var item in _moveToTarget.Coroutine(args))
                {
                    if (args.CancellationToken.IsCancellationRequested)
                    {
                        yield break;
                    }

                    yield return item;
                }

                // начинаем бить, можем не убить - цель убежала
                foreach (var item in UseWeaponCoroutine(args))
                {
                    if (args.CancellationToken.IsCancellationRequested)
                    {
                        yield break;
                    }

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

                if(MathUtils.L1(Host.Cell, Target.Cell) > Host.Attacker.Weapon.ToolDistance)
                {
                    // цель отошла, надо опять к ней идти, да что такое-то
                    yield break;
                }

                bool hit = false;
                uint time = 0;
                foreach (var item in _attack.Coroutine(args))
                {
                    if (args.CancellationToken.IsCancellationRequested)
                    {
                        yield break;
                    }

                    if (!hit)
                    {
                        time += args.DelayMs;
                        if (time > Host.Attacker?.Weapon?.HitMs)
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
            return Target.AttackTarget != null
                && Target.AttackTarget.Hitpoints > 0;
        }

        private void OnHit(EntityCoroutineArgs args)
        {
            Target.AttackTarget.Hitpoints = Math.Max(0, Target.AttackTarget.Hitpoints - Host.Attacker.Weapon.GetDamage(args.Stage, Target));
            Target.SetAttacked(args.Stage, Host, true);
        }

        private bool BeforeHit(EntityCoroutineArgs args)
        { 
            if (!CanAttackEnemy())
            {
                return false;
            }

            _attack.DurationMs = Host.Attacker.Weapon.SwipeMs;
            return true;
        }

        private bool CanAttackEnemy()
        {
            return Target?.AttackTarget != null
                && Host.Attacker?.Weapon?.CanInteract(Target) == true;
        }
    }
}
