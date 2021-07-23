using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities.Stress;
using foxer.Core.Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace foxer.Core.Game.Entities.Descriptors
{
    public abstract class EntityDescriptorBase
    {
        public abstract Type EntityType { get; }

        public virtual IStresser Stresser { get; }

        public EntityKind Kind { get; }

        protected EntityDescriptorBase(EntityKind kind)
        {
            Kind = kind;
        }

        public bool CanBePlaced(Stage stage, EntityBase entity, int x, int y, float z)
        {
            var cell = stage.GetCell(x, y);
            if (cell == null || cell.Kind == CellKind.Bound)
            {
                return false;
            }

            var entites = stage.GetEntitesInCell(x, y).Where(e => e != entity);
            return OnCanBePlaced(stage, cell, entites, z);
        }

        protected virtual bool OnCanBePlaced(Stage stage, CellBase cell, IEnumerable<EntityBase> entites, float z)
        {
            return !entites.Any(e => !stage.GetDescriptor(e.GetType()).CheckCanOtherBePlacedHere(this));
        }

        public virtual ItemBase GetLoot(Stage stage, EntityBase entity)
        {
            return null;
        }

        protected virtual bool CheckCanOtherBePlacedHere(EntityDescriptorBase descriptor)
        {
            //    BC SC BI SI
            // BC  -  +  -  +
            // SC  +  +  -  +
            // BI  -  -  -  -
            // SI  +  +  -  -  

            switch(Kind)
            {
                case EntityKind.BigCreature:
                    // can be placed with small items and small creatures
                    return descriptor.Kind != EntityKind.BigItem
                        && descriptor.Kind != EntityKind.BigCreature;

                case EntityKind.SmallCreature:
                    // can be placed with small items and big/small creatures
                    return descriptor.Kind != EntityKind.BigItem;

                case EntityKind.BigItem:
                    // can't be placed with big/small items and big/small creatures
                    return false;

                case EntityKind.SmallItem:
                    // can be placed with big/small creatures
                    return descriptor.Kind != EntityKind.BigItem
                        && descriptor.Kind != EntityKind.SmallItem;

                default:
                    throw new ArgumentException($"{nameof(EntityDescriptorBase)}.{nameof(CheckCanOtherBePlacedHere)}.{nameof(descriptor)}");
            }
        }
    }
}
