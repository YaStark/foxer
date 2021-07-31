using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace foxer.Core.Game.Attack
{
    public class AttackerBehaviorNeutral : IAttackerAI, IAwareEntitesProvider
    {
        private readonly Dictionary<EntityFighterBase, int> _aggressors = new Dictionary<EntityFighterBase, int>();

        public EntityFighterBase AttackTarget { get; private set; }

        public AttackerAIBehavior CurrentBehavior { get; private set; }

        private int _timeToTickHp;

        public int MaxAggressorsToFight { get; set; } = 1;

        public float HitpointsRateToRunAway { get; set; } = 0.3f;

        public float HitpointsRateToComeBackFight { get; set; } = 0.7f;

        public double RegenerationHpPerSecond { get; set; } = 1;

        public int TimeMsToForgive { get; set; } = 5000;

        public int DistanceToForgive { get; set; } = 10;

        public int DistanceToRunAway { get; set; } = 15;

        public bool OnUpdate(Stage stage, EntityFighterAIBase host, uint delayMs)
        {
            RegenerateHp(host, delayMs);
            UpdateAggressionList(host, delayMs);
            if (!_aggressors.Any())
            {
                AttackTarget = null;
                CurrentBehavior = AttackerAIBehavior.Idle;
                return false;
            }

            return CanFight(host) 
                ? Fight(stage, host)
                : Hide(stage, host);
        }

        public void OnAttacked(EntityFighterBase aggressor)
        {
            _aggressors[aggressor] = TimeMsToForgive;
        }
        
        private bool Fight(Stage stage, EntityFighterAIBase host)
        {
            if (CurrentBehavior == AttackerAIBehavior.Fighting)
            {
                return true;
            }

            AttackTarget = GetAttackTarget(host);
            if(AttackTarget == null)
            {
                Debug.WriteLine("[AttackerBehaviorNeutral] Try begin fight; everybody too far");
                return Hide(stage, host);
            }

            if(host.BeginFight(stage, AttackTarget))
            {
                Debug.WriteLine("[AttackerBehaviorNeutral] Try begin fight; new aim!");
                CurrentBehavior = AttackerAIBehavior.Fighting;
                return true;
            }

            Debug.WriteLine("[AttackerBehaviorNeutral] Not sure what I need to do");
            CurrentBehavior = AttackerAIBehavior.Idle;
            return false;
        }

        private bool Hide(Stage stage, EntityFighterAIBase host)
        {
            if(CurrentBehavior == AttackerAIBehavior.Runaway
                && !host.IsIdle())
            {
                return true;
            }

            Debug.WriteLine("[AttackerBehaviorNeutral] There are enemies nearby, stop fighting and run!");
            AttackTarget = null;
            host.EndFight();

            if (host.BeginRunaway(stage, this))
            {
                Debug.WriteLine("[AttackerBehaviorNeutral] Run!");
                CurrentBehavior = AttackerAIBehavior.Runaway;
                return true;
            }

            Debug.WriteLine("[AttackerBehaviorNeutral] Can't run, fight!");
            return Fight(stage, host);
        }

        private EntityFighterBase GetAttackTarget(EntityFighterBase host)
        {
            if(_aggressors.Count == 1)
            {
                return _aggressors.First().Key;
            }

            if(host.IsIdle() || AttackTarget == null)
            {
                // если сейчас ничего не делает, надо бить ближайшего
                return _aggressors.Keys.OrderBy(e => MathUtils.L1(e.Cell, host.Cell)).FirstOrDefault();
            }
            else
            {
                return AttackTarget;
            }
        }

        private bool CanFight(EntityFighterBase host)
        {
            if(CurrentBehavior == AttackerAIBehavior.Runaway)
            {
                return HitpointsRateToComeBackFight * host.MaxHitpoints < host.Hitpoints
                    && _aggressors.Count <= MaxAggressorsToFight;
            }

            return HitpointsRateToRunAway * host.MaxHitpoints < host.Hitpoints
                && _aggressors.Count <= MaxAggressorsToFight;
        }

        private void RegenerateHp(EntityFighterBase host, uint delayMs)
        {
            if (host.Hitpoints < host.MaxHitpoints
                && host.Hitpoints > 0)
            {
                _timeToTickHp += (int)delayMs;
                if (_timeToTickHp > 1000 / RegenerationHpPerSecond)
                {
                    host.Hitpoints = Math.Min(host.Hitpoints +(int)(_timeToTickHp * RegenerationHpPerSecond / 1000), host.MaxHitpoints);
                    _timeToTickHp = 0;
                }
            }
        }

        private void UpdateAggressionList(EntityFighterAIBase host, uint delayMs)
        {
            foreach (var enemy in _aggressors.Keys.ToArray())
            {
                _aggressors[enemy] -= (int)delayMs;
                if (_aggressors[enemy] < 0
                    || MathUtils.L1(enemy.Cell, host.Cell) >= DistanceToForgive)
                {
                    Debug.WriteLine("[AttackerBehaviorNeutral] Forgive " + enemy.ToString());
                    _aggressors.Remove(enemy);
                    if (AttackTarget == enemy)
                    {
                        CurrentBehavior = AttackerAIBehavior.Idle;
                        AttackTarget = null;
                        Debug.WriteLine("[AttackerBehaviorNeutral] End fight with forgiven");
                        host.EndFight();
                    }
                }
            }
        }

        public IEnumerable<EntityBase> GetAwareEntites()
        {
            return _aggressors.Keys;
        }
    }
}
