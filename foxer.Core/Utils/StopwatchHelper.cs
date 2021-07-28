using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace foxer.Core.Utils
{
    public class StopwatchHelper
    {
        private readonly Stopwatch _stopwatchScope = new Stopwatch();
        private readonly Stopwatch _stopwatchPoint = new Stopwatch();
        private readonly Dictionary<string, TimeSpan> _scopes = new Dictionary<string, TimeSpan>();
        private readonly Dictionary<string, List<long>> _points = new Dictionary<string, List<long>>();

        private string _lastScope;
        private string _lastPoint;

        public string MainScope { get; }

        public StopwatchHelper(string mainScopeName)
        {
            MainScope = mainScopeName;
        }

        public void Scope(string scopeName)
        {
            EndScope();
            _lastScope = scopeName;
            _stopwatchScope.Restart();
        }

        public void Point(string name)
        {
            EndPoint();
            _lastPoint = name;
            _stopwatchPoint.Restart();
        }

        public void EndScope()
        {
            if (!string.IsNullOrEmpty(_lastScope))
            {
                _stopwatchScope.Stop();
                _scopes[_lastScope] = _stopwatchScope.Elapsed;
                _lastScope = null;
            }
        }

        public void EndPoint()
        {
            if (!string.IsNullOrEmpty(_lastPoint))
            {
                _stopwatchPoint.Stop();
                if (_points.TryGetValue(_lastPoint, out var list))
                {
                    list.Add(_stopwatchPoint.ElapsedMilliseconds);
                }
                else
                {
                    _points[_lastPoint] = new List<long>(new[] { _stopwatchPoint.ElapsedMilliseconds });
                }

                _lastPoint = null;
            }
        }

        public void Show()
        {
            EndPoint();
            EndScope();
            return;
            Debug.WriteLine($"##### Begin {MainScope} #####");
            Debug.WriteLine("### Scopes ###");
            foreach (var kv in _scopes)
            {
                Debug.WriteLine($"{kv.Key}: {Math.Round(kv.Value.TotalMilliseconds, 4)} ");
            }

            Debug.WriteLine("### Points ###");
            foreach(var kv in _points)
            {
                Debug.WriteLine($"{kv.Key}: " +
                    $"total {kv.Value.Sum()}; " +
                    $"avg: {Math.Round(kv.Value.Average(), 4)}; " +
                    $"count: {kv.Value.Count} ");
            }

            Debug.WriteLine($"##### End {MainScope} #####");
        }

        public void Clear()
        {
            _scopes.Clear();
            _points.Clear();
        }
    }
}
