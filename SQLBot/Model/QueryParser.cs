using AIMLbot;
using Cindalnet.SQLBot.Database;
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
        protected Bot ChatBot = new Bot();
        protected const string UserId = "SQLBot";
        protected User ChatUser;

        public QueryParser()
        {
            ChatBot.loadSettings();
            ChatUser = new User(UserId, ChatBot);
            ChatBot.loadAIMLFromFiles();
            ChatBot.isAcceptingUserInput = true;
        }

        private string MorfParse(string Query)
        {
            string res = "";
            Cindalnet.SQLBot.Model.MorfeuszDllWrapper.InterpMorf[] items = MorfeuszDllWrapper.ParseQuery(Query);
            int k = -1;
            foreach (var item in items)
            {   // Bierzemy pod uwagę pierwszą interpretację słowa
                if (k != item.k)
                {
                    if (res.Length > 0)
                        res += " ";
                    // Zamiana rzeczowników na ich formy podstawowe
                    if (item.interp.StartsWith("subst"))
                        res += item.haslo;
                    else
                        res += item.forma;
                    k = item.k;
                }
            }
            return res;
        }

        private string TrimWord(string Word)
        {
            if (Word.EndsWith(".") || Word.EndsWith("?") || Word.EndsWith("!"))
                Word = Word.Substring(0, Word.Length - 1);
            return Word.Trim();
        }

        private string queryDatabase(string chatResponse)
        {
            string res = chatResponse;

            var args = res.Split('|');
            if (args.Length > 1)
            {
                try
                {
                    /*
                     * ARGS[0] = DISPLAY
                     * ARGS[1] = informacja o tym co chcemy wyświelić -
                     */
                    for (int argsNum = 1; argsNum < args.Length; argsNum++)
                    {
                        string x = MorfParse(args[argsNum]);
                    }
                }
                catch (Exception)
                {
                    res = "ERROR";
                }
            }

            return res;
        }


        public string ParseQuery(string query)
        {
            string res;
            try
            {
                //query = MorfParse(query);

                Request chatRequest = new Request(query, ChatUser, ChatBot);
                Result chatRes = ChatBot.Chat(chatRequest);
                res = chatRes.Output;

                if (res.StartsWith("DISPLAY|"))
                {   // 1. Odpytanie bazy danych na podstawie znanych informacji
                    // 2. Ponowne zapytanie systemu konwersacyjnego - rekurencja
                    res = ParseQuery(queryDatabase(res));
                }

            }catch(Exception ex)
            {
                res = "Wystąpił błąd podczas przetwarzania zapytania: " + ex.Message;
            }
            return res;
        }


    }
}
