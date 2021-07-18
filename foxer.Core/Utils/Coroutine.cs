using System;
using System.Collections.Generic;
using System.Threading;

namespace foxer.Core.Utils
{
    public class Coroutine<TItem, TArgs> : ICoroutine
        where TArgs : CoroutineArgs, new()
    {
        private IEnumerator<TItem> _coroutine;

        public TArgs Argument { get; } = new TArgs();

        public TItem Current { get; private set; }

        public bool Finished => EqualityComparer<TItem>.Default.Equals(Current, default(TItem));

        public void Update()
        {
            if (_coroutine != null)
            {
                if (_coroutine.MoveNext())
                {
                    Current = _coroutine.Current;
                }
                else
                {
                    Stop();
                }
            }
            else
            {
                Current = default(TItem);
            }
        }

        public void Start(params Func<TArgs, IEnumerable<TItem>>[] coroutines)
        {
            _coroutine = null;
            if (Argument.CancellationToken != null)
            {
                DisposeCancellationToken(true);
            }

            Argument.CancellationToken = new CancellationTokenSource();

            if (coroutines.Length > 1)
            {
                _coroutine = ConcatCoroutines(coroutines).Invoke(Argument).GetEnumerator();
            }
            else
            {
                _coroutine = coroutines[0].Invoke(Argument).GetEnumerator();
            }
        }

        public void Stop()
        {
            _coroutine = null;
            Current = default(TItem);
            DisposeCancellationToken(true);
        }

        private Func<TArgs, IEnumerable<TItem>> ConcatCoroutines(Func<TArgs, IEnumerable<TItem>>[] coroutines)
        {
            return args => ConcatCoroutines(args, coroutines);
        }

        private static IEnumerable<TItem> ConcatCoroutines(TArgs args, Func<TArgs, IEnumerable<TItem>>[] coroutines)
        {
            foreach (var coroutine in coroutines)
            {
                if(args.CancellationToken.IsCancellationRequested)
                {
                    yield break;
                }

                foreach (var item in coroutine(args))
                {
                    yield return item;
                }
            }
        }

        private void DisposeCancellationToken(bool cancel)
        {
            if (cancel)
            {
                Argument.CancellationToken?.Cancel();
            }

            Argument.CancellationToken?.Dispose();
            Argument.CancellationToken = null;
        }
    }
}
