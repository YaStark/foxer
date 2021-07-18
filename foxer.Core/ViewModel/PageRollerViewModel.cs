using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace foxer.Core.ViewModel
{
    public class PageRollerViewModel : ViewModelBase
    {
        private double _value = 0.5;
        private double _previousValue = 0.5;
        private DelegateCommand _commandMore;
        private DelegateCommand _commandLess;
        private readonly Random _rnd; // todo: to service

        public double Value
        {
            get { return _value; }
            set { SetValue(ref _value, value); }
        }

        public double PreviousValue
        {
            get { return _previousValue; }
            set { SetValue(ref _previousValue, value); }
        }

        public ICommand CommandMore => _commandMore;

        public ICommand CommandLess => _commandLess;

        public PageRollerViewModel()
        {
            _commandMore = new DelegateCommand(() => OnCommand(), true);
            _commandLess = new DelegateCommand(() => OnCommand(), true);
            _rnd = new Random(DateTime.Now.Millisecond);
        }

        private async void OnCommand()
        {
            _commandMore.Enabled = false;
            _commandLess.Enabled = false;

            int steps = 150;
            double from = Value;
            double to = _rnd.NextDouble();
            for (int i = 0; i < steps; i++)
            {
                Value = LerpValue(from, to, (double)(i + 1) / steps, 12);
                await Task.Delay(5);
            }

            if (Value != PreviousValue)
            {
                double steps0 = 100;
                double prev = PreviousValue;
                for (int i = 0; i < steps0; i++)
                {
                    PreviousValue = LerpPrevValue(prev, Value, (double)(i + 1) / steps0);
                    await Task.Delay(10);
                }
            }

            _commandMore.Enabled = true;
            _commandLess.Enabled = true;
        }
        
        // t: 0..1
        // from: 0..1
        // to: 0..1
        // result: 0..1
        private static double LerpValue(double from, double to, double t, int shakes)
        {
            const double xVolatileAmplitude = 0.1;
            double x = t * Math.PI;
            double xVolatileTrend = Math.Pow(x - Math.PI / 2, 2) / (2 * Math.Pow(Math.PI / 2, 2)) + 0.5;
            double xVolatile = Math.Cos(shakes * x) * Math.Sin(-x) * xVolatileAmplitude + xVolatileTrend;
            return ((to - from) * Math.Cos((1 + Math.Cos(x)) * Math.PI / 2) + from + to) * xVolatile / 2;
        }

        private static double LerpPrevValue(double from, double to, double t)
        {
            double x = t * Math.PI;
            return ((from - to) * Math.Cos(x) + from + to) / 2;
        }
    }
}
