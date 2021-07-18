using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using foxer.Core.Game.Entities;
using foxer.Render.Helpers;

namespace foxer.Render
{
    public class TreeRenderer : EntityRendererBase<TreeEntity>
    {
        private static readonly Dictionary<uint, float> _scaleToAge = new Dictionary<uint, float>();
        private static readonly Dictionary<uint, byte[][]> _imageTreesByAge = new Dictionary<uint, byte[][]>();
        private static AnimationRendererHelper _shakeHelper = new AnimationRendererHelper(
            Properties.Resources.tree_leaves_shake_1,
            Properties.Resources.tree_leaves_shake_2,
            Properties.Resources.tree_leaves_shake_3,
            Properties.Resources.tree_leaves_shake_4,
            Properties.Resources.tree_leaves_shake_5,
            Properties.Resources.tree_leaves_shake_6);

        static TreeRenderer()
        {
            _imageTreesByAge.Add(0, new[] {
                Properties.Resources.tree1s,
                Properties.Resources.tree2s,
                Properties.Resources.tree3s,
                Properties.Resources.tree4s
            });

            _imageTreesByAge.Add(TreeEntity.AGE_MEDIUM, new[] {
                Properties.Resources.tree1m,
                Properties.Resources.tree2m,
                Properties.Resources.tree3m,
                Properties.Resources.tree4m
            });

            _imageTreesByAge.Add(TreeEntity.AGE_LARGE, new[] {
                Properties.Resources.tree1,
                Properties.Resources.tree2,
                Properties.Resources.tree3,
                Properties.Resources.tree4
            });

            _scaleToAge.Add(0, 1);
            _scaleToAge.Add(TreeEntity.AGE_MEDIUM, 1.5f);
            _scaleToAge.Add(TreeEntity.AGE_LARGE, 2f);
        }

        protected override void Render(INativeCanvas canvas, TreeEntity entity, RectangleF bounds)
        {
            bounds = GetBounds(entity, bounds);
            canvas.DrawImage(GetImage(entity), bounds);
            if(entity.ActiveAnimation == entity.Shake)
            {
                canvas.DrawImage(_shakeHelper.GetImage(entity.Shake), bounds);
            }
        }

        private byte[] GetImage(TreeEntity entity)
        {
            return GetByKind(entity.Kind, GetByAge(entity.Age, _imageTreesByAge));
        }

        private RectangleF GetBounds(TreeEntity entity, RectangleF proposed)
        {
            return ScaleBounds(proposed, GetByAge(entity.Age, _scaleToAge));
        }

        private static byte[] GetByKind(int kind, byte[][] images)
        {
            var index = kind % images.Length;
            return images[index];
        }

        private static T GetByAge<T>(uint age, Dictionary<uint, T> dict)
        {
            var key = dict.Keys.Where(k => k <= age).Max();
            return dict[key];
        }
    }
}
