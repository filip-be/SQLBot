using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.View
{
    public interface IFormConnectToServer : IForm
    {
        string Server { get; set; }
        string User { get; set; }
        string Database { get; set; }
        string Password { get; set; }

        string Error { set; }
    }
}
