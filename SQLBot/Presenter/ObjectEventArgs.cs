using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Presenter
{
    public class ObjectEventArgs : EventArgs
    {
        private readonly object _obj;

        public ObjectEventArgs(object obj)
        {
            _obj = obj;
        }

        public object Obj
        {
            get { return _obj; }
        }
    }
}
