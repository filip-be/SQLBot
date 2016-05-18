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
                    TramwajeDataContext dc = new TramwajeDataContext();
                    string start = null;
                    string end = null;

                    for (int argNum = 2; argNum < args.Length; argNum++)
                    {   // NazwaParametru_Wartość
                        var par_val = args[argNum].Split('_');
                        if (par_val.Length == 2)
                        {
                            switch (par_val[0])
                            {
                                case "START":
                                    start = MorfParse(TrimWord(par_val[1]));
                                    break;
                                case "END":
                                    end = MorfParse(TrimWord(par_val[1]));
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                    switch (args[1])
                    {
                        case "HOW":
                            var queryResHow = dc.WariantTrasies
                                .Join(dc.Przystaneks, w => w.Id, p => p.WariantId, (w, p) => new { WariantTrasy = w, Przystanek = p })
                                .Where(w => (start == null || w.WariantTrasy.Przystaneks.Any(p => p.Nazwa == start))
                                    && (end == null || w.WariantTrasy.Przystaneks.Any(p => p.Nazwa == end)))
                                .FirstOrDefault();
                            if (queryResHow == null)
                            {
                                res = "NULL";
                            }else
                            {
                                res = queryResHow.WariantTrasy.Linie.Typ.Trim() + " HOW " + queryResHow.WariantTrasy.Linie.Nazwa.Trim();
                            }
                            break;
                        case "WHEN":

                            var queryWhen = from p in dc.Przystaneks
                                               join wt in dc.WariantTrasies on p.WariantId equals wt.Id
                                               join prz in dc.Przyjazds on p.Id equals prz.PrzystanekId
                                               where prz.Godzina >= DateTime.Now.Hour 
                                                    && prz.Minuta >= DateTime.Now.Minute
                                               orderby prz.Godzina ascending, prz.Minuta ascending
                                               select new
                                               {
                                                   Numer = wt.Linie.Nazwa,
                                                   Godzina = prz.Godzina,
                                                   Minuta = prz.Minuta,
                                                   WariantTrasy = wt
                                               };
                            var queryResWhen = queryWhen.Where(w => (start == null || w.WariantTrasy.Przystaneks.Any(p => p.Nazwa == start))
                                    && (end == null || w.WariantTrasy.Przystaneks.Any(p => p.Nazwa == end)))
                                    .FirstOrDefault();

                            if (queryResWhen == null)
                            {
                                res = "NULL";
                            }else
                            {
                                res = queryResWhen.Godzina + ":" + queryResWhen.Minuta + " WHEN " + queryResWhen.Numer.Trim();
                            }
                            break;
                        default:
                            break;
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

                if (res.StartsWith("DO_WORK|"))
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
