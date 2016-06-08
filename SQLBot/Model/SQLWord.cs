using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Model
{
    public class SQLWord
    {
        public enum EWordType
        {
            Value,
            Field,
            Table,
            Unknown
        };

        public string Definition { get; set; }
        public EWordType WordType { get; set; }

        public string SQLColumn { get; set; }
        public string SQLTable { get; set; }

        public SQLWord Parent { get; set; }
        

        public SQLWord()
        {

        }

        
        public bool Initialize(AIMLbot.Bot ChatBot, string FieldName)
        {
            return false;
        }
    }
}
