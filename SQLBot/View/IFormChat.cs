using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.View
{
    public interface IFormChat : IForm
    {
        /// <summary>
        /// Zapytanie użytkownika
        /// </summary>
        string Query { get; set; }
        /// <summary>
        /// Odpowiedź systemu
        /// </summary>
        string Response { set; }

        /// <summary>
        /// Żądanie odpowiedzi przez użytkownika
        /// </summary>
        event EventHandler ProcessMessage;
    }
}
