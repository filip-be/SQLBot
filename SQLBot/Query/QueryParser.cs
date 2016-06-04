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
using Cindalnet.SQLBot.Query;
using Cindalnet.SQLBot.Model;
using Cindalnet.SQLBot.Database;

namespace Cindalnet.SQLBot.Query
{
    public class QueryParser
    {
        protected Bot ChatBot = new Bot();
        protected const string UserId = "SQLBot";
        protected User ChatUser;

        private const string PatternDisplay = "DISPLAY|";
        private const string PatternSQLQuery = "SQL_QUERY|";

        public SQLQueryResult QueryResult { get; set; }

        public QueryParser()
        {
            ChatBot.loadSettings();
            ChatUser = new User(UserId, ChatBot);
            ChatBot.loadAIMLFromFiles();
            ChatBot.isAcceptingUserInput = true;
            QueryResult = null;
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

        private void findField(string name, out string[] sqlNames, out string[] sqlTables)
        {
            List<string> lNames = new List<string>();
            List<string> lTables = new List<string>();
            try
            {
                BazaRelacyjnaDataContext dc = new BazaRelacyjnaDataContext();
                SQLBot_Field[] fields = dc.SQLBot_Field.Where(w => w.sqlf_ColumnName == name).ToArray();
                foreach (var field in fields)
                {
                    lNames.Add(field.sqlf_SQLColumnName);
                    lTables.Add(field.SQLBot_Table.sqlt_SQLName);
                }
            }
            catch (Exception)
            {

            }
            sqlNames = lNames.ToArray();
            sqlTables = lTables.ToArray();
        }

        private void findJoin(string table1, string table2, out string Join)
        {
            try
            {
                BazaRelacyjnaDataContext dc = new BazaRelacyjnaDataContext();
                vSQLBotTableJoins[] tableJoins = dc.vSQLBotTableJoins.Where(tj => 
                    tj.Table1NameSQL == table1 && tj.Table2NameSQL == table2
                    || tj.Table1NameSQL == table2 && tj.Table2NameSQL == table1).ToArray();
                if (tableJoins.Length > 0)
                    Join = tableJoins[0].TableJoin;
                else
                    Join = null;
            }
            catch (Exception)
            {
                Join = null;
            }
        }

        private bool IsTableName(string name, out string[] sqlTables)
        {
            try
            {
                sqlTables = findTable(name);
                if (sqlTables.Length > 0)
                    return true;
                else
                    return false;
            }
            catch(Exception)
            {
                sqlTables = null;
                return false;
            }
        }

        private bool IsFieldName(string name, out string[] sqlNames, out string[] sqlTables)
        {
            try
            {
                findField(name, out sqlNames, out sqlTables);
                if (sqlNames.Length > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                sqlNames = null;
                sqlTables = null;
                return false;
            }
        }

        private bool IsValidJoin(string table1, string table2, out string Join)
        {
            try
            {
                findJoin(table1, table2, out Join);
                if (Join.Length > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                Join = null;
                return false;
            }
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
                        QueryInterpreter queryInterp = new QueryInterpreter(parameters[argsNum]);
                        if (queryInterp.IsInterpreted && queryInterp.DesiredParameter != null)
                        {
                            string field = queryInterp.DesiredParameter;
                            string[] sqlFields, sqlTables;

                            foreach(Word word in queryInterp.Words)
                            {
                                if(word.PartOfSpeech == Word.SpeechPart.Noun)
                                {
                                    sqlTables = null;
                                    sqlFields = null;
                                    if(!IsTableName(word.FormBase, out sqlTables) && !IsFieldName(word.FormBase, out sqlFields, out sqlTables))
                                    {
                                        int x = 0;
                                        x = x + 1;
                                        // Jest niewiadomo czym - trzeba to określić!
                                    }
                                    else
                                    {   // Jest czymś, można porównać tabele - jeśli są rożne, trzeba odwołać się do złączenia!
                                        if(FROM == null || FROM.Length == 0)
                                        {   // Nie ma określonej tabeli wejściowej
                                            if (sqlTables != null&& sqlTables.Length > 0 )
                                                FROM = sqlTables[0];
                                        }
                                        else if(sqlTables != null && sqlTables.Length > 0 && FROM != sqlTables[0])
                                        {
                                            string JoinString;
                                            if(IsValidJoin(sqlTables[0], FROM, out JoinString))
                                            {
                                                JOIN = string.Format("{0} ON {1}", sqlTables[0], JoinString);
                                            }
                                            else
                                            {   // Nieprawidłowe złączenie

                                            }
                                        }

                                        if(word.FormBase == queryInterp.DesiredParameter && sqlFields != null && sqlFields.Length > 0)
                                        {   // Poszukiwane słowo jest nazwą pola
                                            SELECT = sqlFields[0];
                                        }
                                    }
                                }
                            }
                            /*
                            if (IsTableName(queryInterp.DesiredParameter, out sqlTables))   // Sprawdzenie czy poszukiwana wartość jest nazwą tabeli
                            {
                                if (sqlTables.Length > 1)
                                {   // Znaleziono więcej niż jedną tabelę pasującą do tej nazwy

                                }
                                else if (sqlTables.Length == 1)
                                {
                                    FROM = sqlTables[0];
                                }
                            }
                            else if (IsFieldName(queryInterp.DesiredParameter, out sqlFields, out sqlTables))    // Sprawdzenie czy poszukiwana wartość jest nazwą pola
                            {
                                if (sqlFields.Length > 1)
                                {   // Znaleziono więcej niż jedną tabelę pasującą do tej nazwy

                                }
                                else if (sqlFields.Length == 1)
                                {
                                    SELECT = sqlFields[0];
                                    FROM = sqlTables[0];
                                }
                            }
                            */
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
                    res = string.Format("{0} JOIN {1}", res, JOIN);
                }

                if(WHERE.Length > 0)
                {
                    res = string.Format("{0} WHERE {1}", res, WHERE);
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

                    if (SQLQuery.EndsWith(".") || SQLQuery.EndsWith("!") || SQLQuery.EndsWith("?"))
                        SQLQuery = SQLQuery.Remove(SQLQuery.Length - 1);

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

        private string queryDatabase(string chatResponse, bool isCleanSQLQuery)
        {
            string res = chatResponse;

            
            string SQLQuery;
            if(isCleanSQLQuery)
                SQLQuery = chatResponse;
            else
                SQLQuery = prepareQuery(chatResponse);
            
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
                Request chatRequest = new Request(query, ChatUser, ChatBot);
                Result chatRes = ChatBot.Chat(chatRequest);
                res = chatRes.Output;

                if (res.StartsWith(PatternDisplay))
                {   // 1. Odpytanie bazy danych na podstawie znanych informacji
                    // 2. Ponowne zapytanie systemu konwersacyjnego - rekurencja
                    res = ParseQuery(queryDatabase(res, false));
                }
                else if(res.StartsWith(PatternSQLQuery))
                {   // Wywołanie konretnego zapytania SQL
                    res = ParseQuery(queryDatabase(res.Substring(PatternSQLQuery.Length), true));
                }


            }catch(Exception ex)
            {
                res = "Wystąpił błąd podczas przetwarzania zapytania: " + ex.Message;
            }
            return res;
        }


    }
}
