using foxer.Core.Game.Entities;
using System.Drawing;

namespace foxer.Core.Game
{
    public interface IWalkBuilderCell
    {
        int X { get; }
        int Y { get; }
        Point Cell { get; }
        IPlatform Platform { get; }
    }
}
