using System;
using System.Collections.Generic;
using System.Text;

namespace foxer.Core.Interfaces
{
    public interface INavigator
    {
        void ShowMainPage();
        void ShowGamePage();
        bool CanGoBack();
        void Back();
    }
}
