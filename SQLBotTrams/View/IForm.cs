using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cindalnet.SQLBot.View
{
    public interface IForm : IView
    {
        Form FormControl { get; }
    }
}
