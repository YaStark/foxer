using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace foxer.Core.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetValue<T>(ref T prop, T value, [CallerMemberName]string propName = "")
        {
            if(Comparer<T>.Default.Compare(prop, value) != 0)
            {
                prop = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
