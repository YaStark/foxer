using foxer.Core.Utils;
using System;
using System.Linq;

namespace foxer.Core.Game.Entities
{
    public class RoofKindManager
    {
        private readonly Stage _stage;

        public RoofKindManager(Stage stage)
        {
            _stage = stage;
        }

        public void OnAddRoof(EntityBase entity)
        {
            if(!(entity is IRoof roof))
            {
                return;
            }

            var dz = entity.GetHeight() / 2;
            var roofs = GameUtils.Nearest4(entity.Cell)
                .Select(pt => _stage.GetEntitesInCell(pt.X, pt.Y)
                    .FirstOrDefault(e => e.GetType() == entity.GetType() && Math.Abs(e.Z - entity.Z) < dz))
                .Cast<IRoof>()
                .ToArray();

            if(!roofs.Any(e => e != null))
            {
                return;
            }

            if (!TrySetOnCornerBetweenTwo(roofs, roof))
            {
                TrySetOnCornerNearOne(roofs, roof);
            }

            TryFixNeighbours(roofs, roof);
        }

        public void OnAdd(EntityBase entity)
        {
            OnAddRoof(entity);
        }

        public void OnRemove(EntityBase entity)
        {
            //todo
        }

        private void TryFixNeighbours(IRoof[] roofs, IRoof roof)
        {
            for (int i = 0; i < 4; i++)
            {
                if (roofs[i]?.RoofKind != RoofKind.Common)
                {
                    continue;
                }

                int angle1 = (360 + 90 - i * 90) % 360; // 90, 0, 270, 180
                int angle2 = (angle1 + 90) % 360; // 180, 90, 0, 270
                int angle3 = (angle2 + 90) % 360; // 270, 180, 90, 0
                int angle4 = (angle3 + 90) % 360; // 0, 270, 180, 90

                if (roofs[i]?.Rotation == angle2 && Rotation0(roof) == angle1)
                {
                    roofs[i].Rotation = angle1;
                    roofs[i].RoofKind = RoofKind.Outer;
                    continue;
                }

                if (roofs[i]?.Rotation == angle4 && Rotation0(roof) == angle1)
                {
                    roofs[i].RoofKind = RoofKind.Inner;
                    continue;
                }

                if (roofs[i]?.Rotation == angle2 && Rotation1(roof) == angle3)
                {
                    roofs[i].RoofKind = RoofKind.Outer;
                    continue;
                }

                if (roofs[i]?.Rotation == angle4 && Rotation1(roof) == angle3)
                {
                    roofs[i].Rotation = angle3;
                    roofs[i].RoofKind = RoofKind.Inner;
                    continue;
                }
            }
        }

        private bool TrySetOnCornerNearOne(IRoof[] roofs, IRoof roof)
        {
            for (int i = 0; i < 4; i++)
            {
                int angle1 = (360 + 90 - i * 90) % 360; // 90, 0, 270, 180
                int angle2 = (angle1 + 90) % 360; // 180, 90, 0, 270
                int angle3 = (angle2 + 90) % 360; // 270, 180, 90, 0
                int angle4 = (angle3 + 90) % 360; // 0, 270, 180, 90

                if (Rotation1(roofs[i]) == angle1 && roof.Rotation == angle2)
                {
                    roof.Rotation = angle1;
                    roof.RoofKind = RoofKind.Inner;
                    return true;
                }

                if (Rotation0(roofs[i]) == angle3 && roof.Rotation == angle2)
                {
                    roof.RoofKind = RoofKind.Inner;
                    return true;
                }

                if (Rotation1(roofs[i]) == angle1 && roof.Rotation == angle4)
                {
                    roof.RoofKind = RoofKind.Outer;
                    return true;
                }

                if (Rotation0(roofs[i]) == angle3 && roof.Rotation == angle4)
                {
                    roof.RoofKind = RoofKind.Outer;
                    roof.Rotation = angle3;
                    return true;
                }
            }

            return false;
        }

        private bool TrySetOnCornerBetweenTwo(IRoof[] roofs, IRoof roof)
        {
            for (int i = 0; i < 4; i++)
            {
                int angle1 = (360 + 90 - i * 90) % 360; // 90, 0, 270, 180
                int angle2 = (angle1 + 90) % 360; // 180, 90, 0, 270
                int angle3 = (angle2 + 90) % 360; // 270, 180, 90, 0
                int angle4 = (angle3 + 90) % 360; // 0, 270, 180, 90

                int i1 = (i + 1) % 4;
                if (Rotation1(roofs[i]) == angle1 && Rotation0(roofs[i1]) == angle2
                    && (roof.Rotation == angle1 || roof.Rotation == angle2))
                {
                    roof.RoofKind = RoofKind.Inner;
                    roof.Rotation = angle1;
                    return true;
                }

                if (Rotation0(roofs[i]) == angle3 && Rotation1(roofs[i1]) == angle4
                    && (roof.Rotation == angle3 || roof.Rotation == angle4))
                {
                    roof.RoofKind = RoofKind.Outer;
                    roof.Rotation = angle3;
                    return true;
                }
            }

            return false;
        }

        private static int? Rotation0(IRoof roof)
        {
            if (roof == null)
            {
                return null;
            }

            switch (roof.RoofKind)
            {
                default:
                case RoofKind.Common:
                case RoofKind.Inner:
                    return roof.Rotation;
                case RoofKind.Outer:
                    return (roof.Rotation + 270) % 360;
            }
        }

        private static int? Rotation1(IRoof roof)
        {
            if (roof == null)
            {
                return null;
            }

            switch (roof.RoofKind)
            {
                default:
                case RoofKind.Common:
                case RoofKind.Outer:
                    return roof.Rotation;
                case RoofKind.Inner:
                    return (roof.Rotation + 90) % 360;
            }
        }
    }

    public interface IRoof
    {
        int Rotation { get; set; }
        RoofKind RoofKind { get; set; }
    }
}
