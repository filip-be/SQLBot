using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Model
{
    
    public class SQLQueryResult
    {
        public IList<SQLQueryFieldInfo> FieldInfo { get; set; }
        public IList<IList<SQLQueryField>> Row { get; set; }

        public SQLQueryResult()
        {
            FieldInfo = new List<SQLQueryFieldInfo>();
            Row = new List<IList<SQLQueryField>>();
        }


    }
}
