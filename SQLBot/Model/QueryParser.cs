using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Model
{
    public class QueryParser
    {
        public static unsafe string ParseQuery(string query)
        {
            string res;
            try
            {
                Cindalnet.SQLBot.Model.MorfeuszDllWrapper.InterpMorf []items = MorfeuszDllWrapper.ParseQuery(query);
                res = "OK";
            }catch(Exception)
            {
                res = "ERROR";
            }
            return res;
        }
    }
}
