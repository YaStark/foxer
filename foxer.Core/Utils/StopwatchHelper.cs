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
        private long _memoryScope;
        private long _memoryPoint;

        private readonly Dictionary<string, TimeSpan> _scopes = new Dictionary<string, TimeSpan>();
        private readonly Dictionary<string, List<long>> _points = new Dictionary<string, List<long>>();

        private readonly Dictionary<string, long> _scopesGc = new Dictionary<string, long>();
        private readonly Dictionary<string, List<long>> _pointsGc = new Dictionary<string, List<long>>();

        private string _lastScope;
        private string _lastPoint;

        public int Tick { get; set; }

        public string MainScope { get; }

        public StopwatchHelper(string mainScopeName)
        {
            MainScope = mainScopeName;
        }

        public void Scope(string scopeName)
        {
            EndScope();
            _memoryScope = GC.GetTotalMemory(false);
            _lastScope = scopeName;
            _stopwatchScope.Restart();
        }

        public void Point(string name)
        {
            EndPoint();
            _lastPoint = name;
            _memoryPoint = GC.GetTotalMemory(false);
            _stopwatchPoint.Restart();
        }

        public void EndScope()
        {
            if (!string.IsNullOrEmpty(_lastScope))
            {
                _stopwatchScope.Stop();
                _scopesGc[_lastScope] = GC.GetTotalMemory(false) - _memoryScope;
                _scopes[_lastScope] = _stopwatchScope.Elapsed;
                _lastScope = null;
            }
        }

        public void EndPoint()
        {
            if (!string.IsNullOrEmpty(_lastPoint))
            {
                var value = GC.GetTotalMemory(false) - _memoryPoint;
                if (_pointsGc.TryGetValue(_lastPoint, out var listGc))
                {
                    listGc.Add(value);
                }
                else
                {
                    _pointsGc[_lastPoint] = new List<long>(new[] { value });
                }

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

        public void ShowEvery(int ticks)
        {
            Tick++;
            if (Tick > ticks)
            {
                Show();
                Clear();
                Tick = 0;
            }
        }

        public void Show()
        {
            EndPoint();
            EndScope();
//            return;
            Debug.WriteLine($"##### Begin {MainScope} #####");
            Debug.WriteLine("### Scopes ###");
            foreach (var scope in _scopes.Keys)
            {
                Debug.WriteLine($"{scope}: {Math.Round(_scopes[scope].TotalMilliseconds, 4)} (alloc {_scopesGc[scope]})");
            }

            Debug.WriteLine("### Points ###");
            foreach(var pt in _points.Keys)
            {
                Debug.WriteLine($"Exec {pt}: " +
                    $"total {_points[pt].Sum()}; " +
                    $"avg: {Math.Round(_points[pt].Average(), 4)}; " +
                    $"count: {_points[pt].Count} ");

                Debug.WriteLine($"Alloc {pt}: " +
                    $"total {_pointsGc[pt].Sum()}; " +
                    $"avg: {Math.Round(_pointsGc[pt].Average(), 4)}; " +
                    $"count: {_pointsGc[pt].Count} ");
            }

            Debug.WriteLine($"##### End {MainScope} #####");
        }

        public void Clear()
        {
            _scopes.Clear();
            _scopesGc.Clear();
            _points.Clear();
            _pointsGc.Clear();
        }
    }
}
