using foxer.Core.Game.Cells;
using foxer.Core.Game.Entities.Stress;
using foxer.Core.Game.Items;
using foxer.Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public bool CanBePlaced(Stage stage, EntityBase entity, int x, int y, IPlatform platform)
        {
            var cell = stage.GetCell(x, y);
            if (cell == null || cell.Kind == CellKind.Bound)
            {
                return false;
            }

            if (!stage.CheckRoomZOnPlatform(entity, x, y, platform))
            {
                return false;
            }

            var isNotOverlap = stage.CheckNotOverlap(entity, x, y, platform.Level);
            var result = isNotOverlap && OnCanBePlaced(stage, cell, platform);
            return result;
        }

        protected virtual bool OnCanBePlaced(Stage stage, CellBase cell, IPlatform platform)
        {
            return true;
        }

        public virtual ItemBase GetLoot(Stage stage, EntityBase entity)
        {
            return null;
        }

        public virtual bool CheckCanOtherBePlacedHere(EntityDescriptorBase descriptor)
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
