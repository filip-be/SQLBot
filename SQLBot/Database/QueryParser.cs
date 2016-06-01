using AIMLbot;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Cindalnet.SQLBot.Database;
using Cindalnet.SQLBot.Model;

namespace Cindalnet.SQLBot.Database
{
    public class QueryParser
    {
        protected Bot ChatBot = new Bot();
        protected const string UserId = "SQLBot";
        protected User ChatUser;

        public SQLQueryResult QueryResult { get; set; }

        public QueryParser()
        {
            ChatBot.loadSettings();
            ChatUser = new User(UserId, ChatBot);
            ChatBot.loadAIMLFromFiles();
            ChatBot.isAcceptingUserInput = true;
            QueryResult = null;
        }

        private string MorfParse(string Query, bool ignoreDot)
        {
            string res = "";
            try
            {
                MorfeuszDllWrapper.InterpMorf[] items = MorfeuszDllWrapper.ParseQuery(Query);
                int k = -1;
                foreach (var item in items)
                {   // Bierzemy pod uwagę pierwszą interpretację słowa
                    if (k != item.k && (!ignoreDot || item.haslo != "."))
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
            }
            catch(Exception)
            {
                res = "ERROR";
            }
            return res;
        }

        private string MorfParse(string Query)
        {
            return MorfParse(Query, false);
        }

        private string TrimWord(string Word)
        {
            if (Word.EndsWith(".") || Word.EndsWith("?") || Word.EndsWith("!"))
                Word = Word.Substring(0, Word.Length - 1);
            return Word.Trim();
        }

        private string []findTable(string name)
        {
            List<string> res = new List<string>();
            try
            {
                BazaRelacyjnaDataContext dc = new BazaRelacyjnaDataContext();
                SQLBot_Table []table = dc.SQLBot_Table.Where(w => w.sqlt_Name == name).ToArray();
                foreach(var tab in table)
                {
                    res.Add(tab.sqlt_SQLName);
                }
            }
            catch(Exception)
            {

            }

            return res.ToArray();
        }

        private string prepareQuery(string chatResponse)
        {
            string res = "";

            string SELECT = "*";
            string FROM = "";
            string JOIN = "";
            string WHERE = "";

            var parameters = TrimWord(chatResponse).Split('|');
            if (parameters.Length > 1)
            {
                try
                {
                    /*
                     * ARGS[0] = DISPLAY
                     * ARGS[1] = informacja o tym co chcemy wyświelić
                     * ARGS[2...] = kryteria
                     */
                    for (int argsNum = 1; argsNum < parameters.Length; argsNum++)
                    {
                        string field = MorfParse(parameters[argsNum], true);

                        string[] tables = findTable(field);
                        if (tables == null || tables.Length == 0)
                        {

                        }
                        else if (tables.Length > 1)
                        {

                        }
                        else if (tables.Length == 1)
                        {
                            FROM = tables[0];
                        }
                    }
                }
                catch (Exception)
                {
                    res = "ERROR - PARSING PARAMETERS";
                }
            }
            else
            {
                return "ERROR - NO PARAMETERS";
            }

            if(res != "ERROR")
            {
                res = string.Format("SELECT {0} FROM {1} ", SELECT, FROM);
                if(JOIN.Length > 0)
                {
                    res = string.Format("{0} JOIN ", JOIN);
                }

                if(WHERE.Length > 0)
                {
                    res = string.Format("{0} WHERE ", WHERE);
                }
            }

            return res;
        }

        private bool executeQuery(string SQLQuery)
        {
            bool res = false;
            try
            {
                SqlConnection con = new SqlConnection(Properties.Settings.Default.BazaRelacyjnaConnectionString);
                {
                    con.Open();
                    DbCommand cmd = con.CreateCommand();
                    cmd.CommandText = SQLQuery;

                    // Create a reader that contains rows of entity data. 
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        QueryResult = new SQLQueryResult();

                        // Pobierz typy poszczególnych pól
                        for (int fieldNum = 0; fieldNum < rdr.FieldCount; fieldNum++)
                        {
                            QueryResult.FieldInfo.Add(
                                new SQLQueryFieldInfo(rdr.GetName(fieldNum), rdr.GetFieldType(fieldNum))
                                );
                        }


                        while (rdr.Read())
                        {
                            IList<SQLQueryField> row = new List<SQLQueryField>();
                            for (int fieldNum = 0; fieldNum < rdr.FieldCount; fieldNum++)
                            {
                                row.Add(new SQLQueryField(rdr.GetValue(fieldNum).ToString(), rdr.GetValue(fieldNum)));
                            }
                            QueryResult.Row.Add(row);
                        };

                        res = true;
                    }
                    con.Close();
                }
            }
            catch(Exception)
            {

            }
            return res;
        }

        private string queryDatabase(string chatResponse)
        {
            string res = chatResponse;

            string SQLQuery = prepareQuery(chatResponse);
            bool QueryResult = executeQuery(SQLQuery);

            res = string.Format("QUERYDONE| {0} | {1}",
                                    QueryResult ? "OK" : "ERROR",
                                    SQLQuery);

            return res;
        }


        public string ParseQuery(string query)
        {
            if (!query.StartsWith("QUERYDONE| OK |"))
            {
                QueryResult = null;
            }

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
