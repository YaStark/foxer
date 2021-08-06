using foxer.Core.Game.Entities;
using foxer.Core.Utils;
using System.Collections.Generic;

namespace foxer.Core.Game.Animation
{
    public abstract class EntityAnimation
    {
        private readonly List<EntityCoroutineDelegate> _before = new List<EntityCoroutineDelegate>();
        private readonly List<EntityCoroutineDelegate> _after = new List<EntityCoroutineDelegate>();

        public double Progress { get; set; }

        public IEnumerable<EntityAnimation> Coroutine(EntityCoroutineArgs args)
        {
            foreach (var coroutine in _before)
            {
                foreach (var item in coroutine.Invoke(args))
                {
                    if (args.CancellationToken.IsCancellationRequested)
                    {
                        yield break;
                    }

                    yield return item;
                }
            }

            foreach(var item in OnCoroutine(args))
            {
                if (args.CancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                yield return item;
            }

            foreach (var coroutine in _after)
            {
                foreach (var item in coroutine.Invoke(args))
                {
                    if (args.CancellationToken.IsCancellationRequested)
                    {
                        yield break;
                    }

                    yield return item;
                }
            }
        }

        public void AlwaysRunBefore(params EntityCoroutineDelegate[] coroutines)
        {
            _before.AddRange(coroutines);
        }

        public void AlwaysRunAfter(params EntityCoroutineDelegate[] coroutines)
        {
            _after.AddRange(coroutines);
        }

        protected abstract IEnumerable<EntityAnimation> OnCoroutine(EntityCoroutineArgs args);
    }
}
