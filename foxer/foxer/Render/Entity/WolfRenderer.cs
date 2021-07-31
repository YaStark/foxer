using foxer.Core.Game.Entities;
using foxer.Properties;
using foxer.Render.Helpers;
using System.Collections.Generic;
using System.Drawing;

namespace foxer.Render
{
    public class WolfRenderer : EntityRendererBase<WolfEntity>
    {
        private static readonly RotatingAnimationRendererHelper _walking = new RotatingAnimationRendererHelper();
        private static readonly RotatingAnimationRendererHelper _sitting = new RotatingAnimationRendererHelper();
        private static readonly RotatingAnimationRendererHelper _standing = new RotatingAnimationRendererHelper();
        private static readonly RotatingAnimationRendererHelper _attack = new RotatingAnimationRendererHelper();
        private static readonly Dictionary<EntityBodyState, RotatingRendererHelper> _idle = new Dictionary<EntityBodyState, RotatingRendererHelper>();

        static WolfRenderer()
        {
            _sitting.AddImagesForRotation(0, Resources.wolf0_sitting_1, Resources.wolf0_sitting_2, Resources.wolf0_sit);
            _sitting.AddImagesForRotation(270, Resources.wolf270_sitting_1, Resources.wolf270_sitting_2, Resources.wolf270_sit);

            _standing.AddImagesForRotation(0, Resources.wolf0_sit, Resources.wolf0_sitting_2, Resources.wolf0_sitting_1);
            _standing.AddImagesForRotation(270, Resources.wolf270_sit, Resources.wolf270_sitting_2, Resources.wolf270_sitting_1);

            _walking.AddImagesForRotation(0,
                Resources.wolf0_walk_1,
                Resources.wolf0_walk_2,
                Resources.wolf0_walk_3,
                Resources.wolf0_walk_4);

            _walking.AddImagesForRotation(90,
                Resources.wolf90_walk_1,
                Resources.wolf90_walk_2,
                Resources.wolf90_walk_3,
                Resources.wolf90_walk_4);

            _walking.AddImagesForRotation(180,
                Resources.wolf180_walk_1,
                Resources.wolf180_walk_2,
                Resources.wolf180_walk_3,
                Resources.wolf180_walk_4);

            _walking.AddImagesForRotation(270,
                Resources.wolf270_walk_1,
                Resources.wolf270_walk_2,
                Resources.wolf270_walk_3,
                Resources.wolf270_walk_4);

            _attack.AddImagesForRotation(0,
                Resources.wolf0_attack_1,
                Resources.wolf0_attack_2,
                Resources.wolf0_attack_3,
                Resources.wolf0_attack_4);

            _attack.AddImagesForRotation(90,
                Resources.wolf90_attack_1,
                Resources.wolf90_attack_2,
                Resources.wolf90_attack_3,
                Resources.wolf90_attack_4);

            _attack.AddImagesForRotation(180,
                Resources.wolf180_attack_1,
                Resources.wolf180_attack_2,
                Resources.wolf180_attack_3,
                Resources.wolf180_attack_4);

            _attack.AddImagesForRotation(270,
                Resources.wolf270_attack_1,
                Resources.wolf270_attack_2,
                Resources.wolf270_attack_3,
                Resources.wolf270_attack_4);

            _idle[EntityBodyState.Stand] = new RotatingRendererHelper();
            _idle[EntityBodyState.Stand].AddImageForRotation(0, Resources.wolf0);
            _idle[EntityBodyState.Stand].AddImageForRotation(90, Resources.wolf90);
            _idle[EntityBodyState.Stand].AddImageForRotation(180, Resources.wolf180);
            _idle[EntityBodyState.Stand].AddImageForRotation(270, Resources.wolf270);

            _idle[EntityBodyState.Sit] = new RotatingRendererHelper();
            _idle[EntityBodyState.Sit].AddImageForRotation(0, Resources.wolf0_sit);
            _idle[EntityBodyState.Sit].AddImageForRotation(270, Resources.wolf270_sit);

        }

        protected override void Render(INativeCanvas canvas, WolfEntity entity, RectangleF bounds)
        {
            canvas.DrawImage(GetImage(entity), ScaleBounds(bounds, 1.5f));
        }

        private byte[] GetImage(WolfEntity entity)
        {
            if (entity.ActiveAnimation == entity.Walk)
            {
                return _walking.GetImageByRotation(entity.Rotation, entity.Walk);
            }

            if (entity.ActiveAnimation == entity.Run)
            {
                return _walking.GetImageByRotation(entity.Rotation, entity.Run);
            }

            if (entity.ActiveAnimation == entity.Sit)
            {
                return _sitting.GetImageByRotation(entity.Rotation, entity.Sit);
            }

            if (entity.ActiveAnimation == entity.Stand)
            {
                return _standing.GetImageByRotation(entity.Rotation, entity.Stand);
            }

            if(entity.ActiveAnimation == entity.Attack)
            {
                return _attack.GetImageByRotation(entity.Rotation, entity.Attack);
            }

            return _idle[entity.State].GetImageByRotation(entity.Rotation);
        }
    }
}
