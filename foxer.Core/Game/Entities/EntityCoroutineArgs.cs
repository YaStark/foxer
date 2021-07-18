using foxer.Core.Utils;

namespace foxer.Core.Game.Entities
{
    public class EntityCoroutineArgs : CoroutineArgs
    {
        public uint DelayMs { get; set; }
        public Stage Stage { get; set; }
    }
}
