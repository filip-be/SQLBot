using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Cindalnet.SQLBot.View
{
    public interface IView : INotifyPropertyChanged
    {
        event EventHandler Cancel;
        event EventHandler OK;
        event EventHandler ViewClosing;
        event EventHandler ViewClosed;
    }
}
