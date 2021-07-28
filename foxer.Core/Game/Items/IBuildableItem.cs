using foxer.Core.Game.Entities;
using System;
using System.Drawing;

namespace foxer.Core.Game.Items
{
    public interface IBuildableItem
    {
        Type EntityType { get; }
        IPlatform GetTopPlatform(Stage stage, int x, int y);
        EntityBase Create(Stage stage, int x, int y, float z);
        EntityBase CreatePreviewItem(int x0, int y0, int x, int y);
        bool CheckCanBuild(Stage stage, int x, int y, float z);
        bool CheckBuildDistance(Point player, Point target);

        // options
        bool UseRotation();
        void RotatePreview(int angle360);
    }
}
