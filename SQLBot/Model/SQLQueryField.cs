using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Model
{
    public class SQLQueryField
    {
        public string Text { get; protected set; }
        public object Value { get; protected set; }

        public SQLQueryField(string _Text, object _Value)
        {
            this.Text = _Text;
            this.Value = _Value;
        }
    }
}
