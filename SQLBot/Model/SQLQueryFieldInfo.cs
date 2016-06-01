using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Model
{
    public class SQLQueryFieldInfo
    {
        public string Name { get; protected set; }
        public Type Type { get; protected set; }

        public SQLQueryFieldInfo(string _Name, Type _Type)
        {
            this.Name = _Name;
            this.Type = _Type;
        }
    }
}
