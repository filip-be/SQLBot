using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.View
{
    public interface IFormManage : IForm
    {
        string FilePath { get; set; }
        event EventHandler AddToDatabase;
        string Message { set; }
    }
}
