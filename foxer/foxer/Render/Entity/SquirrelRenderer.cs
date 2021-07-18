using System.Drawing;
using foxer.Core.Game.Entities;
using foxer.Render.Helpers;

namespace foxer.Render
{
    public class SquirrelRenderer : EntityRendererBase<SquirrelEntity>
    {
        private static readonly RotatingRendererHelper _idle = new RotatingRendererHelper();
        private static readonly RotatingAnimationRendererHelper _run = new RotatingAnimationRendererHelper();

        static SquirrelRenderer()
        {
            _idle.AddImageForRotation(0, Properties.Resources.squirrel0);
            _idle.AddImageForRotation(90, Properties.Resources.squirrel90);
            _idle.AddImageForRotation(180, Properties.Resources.squirrel180);
            _idle.AddImageForRotation(270, Properties.Resources.squirrel270);

            _run.AddImagesForRotation(0,
                Properties.Resources.squirrel0_run_1,
                Properties.Resources.squirrel0_run_2,
                Properties.Resources.squirrel0_run_3,
                Properties.Resources.squirrel0_run_4);

            _run.AddImagesForRotation(90,
                Properties.Resources.squirrel90_run_1,
                Properties.Resources.squirrel90_run_2,
                Properties.Resources.squirrel90_run_3,
                Properties.Resources.squirrel90_run_4);

            _run.AddImagesForRotation(180,
                Properties.Resources.squirrel180_run_1,
                Properties.Resources.squirrel180_run_2,
                Properties.Resources.squirrel180_run_3,
                Properties.Resources.squirrel180_run_4);

            _run.AddImagesForRotation(270,
                Properties.Resources.squirrel270_run_1,
                Properties.Resources.squirrel270_run_2,
                Properties.Resources.squirrel270_run_3,
                Properties.Resources.squirrel270_run_4);
        }

        protected override void Render(INativeCanvas canvas, SquirrelEntity entity, RectangleF bounds)
        {
            if(entity.ActiveAnimation == entity.Hide)
            {
                return;
            }

            if(entity.ActiveAnimation == entity.Run)
            {
                canvas.DrawImage(_run.GetImageByRotation(entity.Rotation, entity.Run), bounds);
            }
            else
            {
                canvas.DrawImage(_idle.GetImageByRotation(entity.Rotation), bounds);
            }
        }
    }
}
