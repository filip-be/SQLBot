using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cindalnet.SQLBot.View
{
    public interface IFormMain : IForm
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
