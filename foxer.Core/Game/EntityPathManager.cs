using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace foxer.Core.Game
{
    public class EntityPathManager
    {
        private class Rule
        {
            public bool Default { get; }
            public HashSet<int> Excepts { get; }

            public Rule(bool defaultFlag)
            {
                Default = defaultFlag;
                Excepts = new HashSet<int>();
            }
        }

        private class RuleBuilder<T>
        {
            private readonly Rule _walkableInfo;

            public RuleBuilder(Dictionary<int, Rule> dict, bool defaultWalkable)
            {
                _walkableInfo = new Rule(defaultWalkable);
                dict.Add(GetTypecode<T>(), _walkableInfo);
            }

            public RuleBuilder<T> AddExcept<TExcept>()
            {
                _walkableInfo.Excepts.Add(GetTypecode<TExcept>());
                return this;
            }
        }

        private static readonly Dictionary<Type, int> _typecodes = new Dictionary<Type, int>();
        private static readonly Dictionary<int, Rule> _walkableRules = new Dictionary<int, Rule>();
        private static readonly Dictionary<int, Rule> _creationRules = new Dictionary<int, Rule>();

        private readonly Stage _stage;

        static EntityPathManager()
        {
            // walkables

            AddWalkableRule<TreeEntity>(false)
                .AddExcept<BeeEntity>()
                .AddExcept<SquirrelEntity>();

            AddWalkableRule<StoneBigEntity>(false)
                .AddExcept<BeeEntity>();

            AddWalkableRule<StoneOvenEntity>(false)
                .AddExcept<BeeEntity>();

            // creation

            AddCreationRule<StoneOvenEntity>(false)
                .AddExcept<BeeEntity>();

            AddCreationRule<TreeEntity>(false)
                .AddExcept<BeeEntity>()
                .AddExcept<SquirrelEntity>();

            AddCreationRule<StoneBigEntity>(false)
                .AddExcept<BeeEntity>();

            AddCreationRule<StoneSmallEntity>(true)
                .AddExcept<StoneBigEntity>()
                .AddExcept<GrassEntity>()
                .AddExcept<FlowerEntity>()
                .AddExcept<TreeEntity>();

            AddCreationRule<FlowerEntity>(true)
                .AddExcept<GrassEntity>()
                .AddExcept<StoneBigEntity>()
                .AddExcept<StoneSmallEntity>()
                .AddExcept<TreeEntity>();

            AddCreationRule<GrassEntity>(true)
                .AddExcept<FlowerEntity>()
                .AddExcept<StoneBigEntity>()
                .AddExcept<StoneSmallEntity>()
                .AddExcept<TreeEntity>();
        }

        public EntityPathManager(Stage stage)
        {
            _stage = stage;
        }

        public bool CanWalkBy(EntityBase entity, int x, int y)
        {
            var byEntites = !_stage.GetEntitesInCell(x, y)
                .Any(e => !CheckRule(_walkableRules, e, entity, true));

            switch (entity)
            {
                default: return byEntites && _stage.GetCell(x, y)?.CanWalk == true;
            }
        }

        public bool CanBeCreated(EntityBase entity, int x, int y)
        {
            var byEntites = !_stage.GetEntitesInCell(x, y)
                .Any(e => !CheckRule(_creationRules, entity, e, true));
            switch(entity)
            {
                case TreeEntity _:
                    return byEntites && _stage.Cells[x, y]?.Kind == CellKind.Floor;

                default:
                    return byEntites;
            }
        }

        private bool CheckRule(Dictionary<int, Rule> rules, object t1, object t2, bool defaultValue)
        {
            if (!_typecodes.TryGetValue(t1.GetType(), out var t1code)
                || !rules.TryGetValue(t1code, out var rule))
            {
                return defaultValue;
            }

            if(!_typecodes.TryGetValue(t2.GetType(), out var t2code)
                || !rule.Excepts.Contains(t2code))
            {
                return rule.Default;
            }

            return !rule.Default;
        }

        private static int GetTypecode<T>()
        {
            return GetTypecode(typeof(T));
        }

        private static int GetTypecode(Type t)
        {
            if (_typecodes.TryGetValue(t, out var code))
            {
                return code;
            }

            _typecodes[t] = _typecodes.Count;
            return _typecodes[t];
        }

        private static RuleBuilder<T> AddWalkableRule<T>(bool defaultWalkable)
        {
            return new RuleBuilder<T>(_walkableRules, defaultWalkable);
        }

        private static RuleBuilder<T> AddCreationRule<T>(bool defaultCanBeCreated)
        {
            return new RuleBuilder<T>(_creationRules, defaultCanBeCreated);
        }
    }
}
