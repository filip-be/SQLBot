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

        private const string PatternPrepareQuery = "SQLBOT APP PREPARE QUERY ";
        private const string PatternSQLQuery = "SQLBOT APP QUERY ";
        private const string PatternGetFormBase = "SQLBOT APP GET FORM BASE ";

        public SQLQueryResult QueryResult { get; set; }

        public QueryParser()
        {
            ChatBot.loadSettings();
            ChatUser = new User(UserId, ChatBot);
            ChatBot.loadAIMLFromFiles();
            ChatBot.isAcceptingUserInput = true;
            QueryResult = null;

            UpdateAIMLKnowledge();
        }

        void UpdateAIMLKnowledge()
        {
            try
            {
                BazaRelacyjnaDataContext dc = new BazaRelacyjnaDataContext();
                SQLBot_Field[] fields = dc.SQLBot_Field.ToArray();
                SQLBot_Table[] tables = dc.SQLBot_Table.ToArray();

                foreach (var field in fields)
                {
                    ChatBot.Chat(string.Format("SQLBOT LEARN WHAT IS {0} | {1} | FIELD", field.sqlf_ColumnName, field.sqlf_Description),
                        ChatUser.UserID);
                }
                foreach (var table in tables)
                {
                    ChatBot.Chat(string.Format("SQLBOT LEARN WHAT IS {0} | {1} | TABLE", table.sqlt_Name, table.sqlt_Description),
                        ChatUser.UserID);
                }
            }
            catch(Exception)
            {

            }
        }

        private string TrimWord(string Word)
        {
            if (Word.EndsWith(".") || Word.EndsWith("?") || Word.EndsWith("!"))
                Word = Word.Substring(0, Word.Length - 1);
            return Word.Trim();
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

        private class QueryExceptionUnknownParameter : Exception
        {
            public QueryExceptionUnknownParameter(string _ParameterName)
            {
                this.ParameterName = _ParameterName;
            }

            public string ParameterName { get; set; }
        }

        private string prepareQuery(string chatResponse)
        {
            string res = "";

            string SELECT = "*";
            string FROM = "";
            string JOIN = "";
            string WHERE = "";

            string Table = "";
            List<SQLWord> LColumns = new List<SQLWord>();
            List<SQLWord> LFrom = new List<SQLWord>();
            List<SQLWord> LJoin = new List<SQLWord>();
            List<SQLWord> LWhere = new List<SQLWord>();

            var parameters = TrimWord(chatResponse).Split('|');
            if (parameters.Length > 1)
            {
                try
                {
                    /*
                     * ARGS[0] = SQLBOT APP PREPARE QUERY
                     * ARGS[1] = informacja o tym co chcemy wyświelić
                     * ARGS[2...] = kryteria
                     */
                    for (int argsNum = 1; argsNum < parameters.Length; argsNum++)
                    {
                        QueryInterpreter queryInterp = new QueryInterpreter(parameters[argsNum]);
                        if (queryInterp.IsInterpreted 
                            && queryInterp.DesiredParameter != null)
                        {
                            string field = queryInterp.DesiredParameter;
                            ///string[] sqlFields, sqlTables;
                            List<Tuple<int, string>> unknownWords = new List<Tuple<int, string>>();

                            for(int wordNum = 0; wordNum < queryInterp.Words.Count; wordNum++)
                            {
                                Word word = queryInterp.Words[wordNum];
                                if(word.PartOfSpeech == Word.SpeechPart.Noun)
                                {
                                    //sqlTables = null;
                                    //sqlFields = null;
                                    string FieldName = word.FormBase;

                                    SQLWord sqlWord = new SQLWord();
                                    sqlWord.Initialize(ChatBot, ChatUser, FieldName);

                                    bool isKnown = sqlWord.isValidWord();

                                    if (!isKnown)
                                    {
                                        Tuple<int, string> unknownWord;

                                        if(unknownWords.Count > 0 
                                            && unknownWords.Last().Item1 + 1 == wordNum)
                                        {
                                            unknownWord = unknownWords.Last();
                                            unknownWords.RemoveAt(unknownWords.Count - 1);
                                            unknownWord = new Tuple<int, string>(wordNum, unknownWord.Item2 + " " + FieldName);
                                            sqlWord = new SQLWord();
                                            sqlWord.Initialize(ChatBot, ChatUser, unknownWord.Item2);
                                            isKnown = sqlWord.isValidWord();
                                        }
                                        else
                                        {
                                            unknownWord = new Tuple<int, string>(wordNum, FieldName);
                                        }
                                        if(!isKnown)
                                            unknownWords.Add(unknownWord);
                                    }
                                    
                                    if(isKnown)
                                    {
                                        if (sqlWord.isValidColumn())
                                        {
                                            Request chatRequest = new Request(string.Format("SQLBOT AIML FIELD PUSH {0}", sqlWord.SQLColumn),
                                                ChatUser, ChatBot);
                                            Result chatRes = ChatBot.Chat(chatRequest);
                                        }
                                        else if (sqlWord.isValidValue())
                                        {
                                            Request chatRequest = new Request(string.Format("SQLBOT AIML VALUE PUSH {0}='{1}'", sqlWord.SQLColumn, sqlWord.Word),
                                                ChatUser, ChatBot);
                                            Result chatRes = ChatBot.Chat(chatRequest);
                                        }


                                        if (sqlWord.SQLTable != null)
                                        {
                                            if (Table.Length == 0)
                                            {
                                                Table = sqlWord.SQLTable;
                                            }
                                            else if (Table != sqlWord.SQLTable)
                                            {
                                                string JoinString;
                                                if (IsValidJoin(Table, sqlWord.SQLTable, out JoinString))
                                                {

                                                    Request chatRequest = new Request(string.Format("SQLBOT AIML JOIN PUSH {0} ON {1}", sqlWord.SQLTable, JoinString),
                                                        ChatUser, ChatBot);
                                                    Result chatRes = ChatBot.Chat(chatRequest);
                                                }
                                                else
                                                {   // Nieprawidłowe złączenie

                                                }
                                            }
                                        }                                        

                                        /*
                                        // Jest czymś, można porównać tabele - jeśli są rożne, trzeba odwołać się do złączenia!
                                        if (FROM == null || FROM.Length == 0)
                                        {   // Nie ma określonej tabeli wejściowej
                                            if (sqlTables != null 
                                                && sqlTables.Length > 0)
                                                FROM = sqlTables[0];
                                        }
                                        else if (sqlTables != null 
                                            && sqlTables.Length > 0 
                                            && FROM != sqlTables[0])
                                        {
                                            string JoinString;
                                            if (IsValidJoin(sqlTables[0], FROM, out JoinString))
                                            {
                                                JOIN = string.Format("{0} ON {1}", sqlTables[0], JoinString);
                                            }
                                            else
                                            {   // Nieprawidłowe złączenie

                                            }
                                        }

                                        if (sqlFields != null 
                                            && sqlFields.Length > 0)
                                        {   // Poszukiwane słowo jest nazwą pola
                                            if(word.FormBase == queryInterp.DesiredParameter)
                                                SELECT = sqlFields[0];
                                            else if (queryInterp.DesiredParameterIndex < wordNum 
                                                && wordNum > 0
                                                && queryInterp.Words[wordNum - 1].PartOfSpeech == Word.SpeechPart.Conjuctiun)
                                            {
                                                SELECT += sqlFields[0];
                                            }
                                        }
                                        */
                                    }
                                }
                                /*
                                else if(
                                    (word.PartOfSpeech == Word.SpeechPart.Conjuctiun
                                    && wordNum > 0
                                    && queryInterp.Words[wordNum-1].PartOfSpeech == Word.SpeechPart.Noun
                                    && (wordNum + 1) < queryInterp.Words.Count
                                    && queryInterp.Words[wordNum+1].PartOfSpeech == Word.SpeechPart.Noun))
                                {
                                    SELECT += ", ";
                                }
                                */
                            }

                            if(unknownWords.Count > 0)
                            {
                                throw new QueryExceptionUnknownParameter(unknownWords.First().Item2);
                            }
                        }
                    }
                }
                catch(QueryExceptionUnknownParameter exP)
                {
                    throw exP;
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

            if(!res.StartsWith("ERROR"))
            {
                res = "SQLBOT BUILD QUERY";
                /*
                res = string.Format("SELECT {0} FROM {1} ", SELECT, FROM);
                if(JOIN.Length > 0)
                {
                    res = string.Format("{0} JOIN {1}", res, JOIN);
                }

                if(WHERE.Length > 0)
                {
                    res = string.Format("{0} WHERE {1}", res, WHERE);
                }
                */
            }

            return res;
        }

        private bool executeQuery(string SQLQuery)
        {
            bool res = false;
            try
            {
                SqlConnection con = new SqlConnection(Properties.Settings.Default.BazaRelacyjnaCustomConnectionString);
                {
                    con.Open();
                    DbCommand cmd = con.CreateCommand();

                    cmd.CommandText = TrimWord(SQLQuery);

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

            try
            {
                string SQLQuery;
                if (isCleanSQLQuery)
                    SQLQuery = chatResponse;
                else
                    SQLQuery = prepareQuery(chatResponse);

                bool QueryResult = executeQuery(SQLQuery);

                res = string.Format("QUERYDONE | {0} | {1}",
                                        QueryResult ? "OK" : "ERROR",
                                        SQLQuery);
            }
            catch(QueryExceptionUnknownParameter ex)
            {
                res = string.Format("SQLBOT UNKNOWN PARAMETER {0}", ex.ParameterName);
            }

            return res;
        }

        public string getFormBase(string sentence)
        {
            try
            {
                string[] sentenceAndResponse = TrimWord(sentence).Split(new[] { "CSPLITSENTENCE" }, StringSplitOptions.None);
                if (sentenceAndResponse.Length > 1)
                {
                    QueryInterpreter queryInterp = new QueryInterpreter(sentenceAndResponse[0]);
                    if (queryInterp.IsInterpreted)
                    {
                        string outputString = sentenceAndResponse[1].Replace("CSTAR", queryInterp.AsStringOfBase);
                        for (int num = 2; num < sentenceAndResponse.Length; num++)
                        {
                            try
                            {
                                Request chatRequest = new Request(sentenceAndResponse[num], ChatUser, ChatBot);
                                Result chatRes = ChatBot.Chat(chatRequest);
                                outputString += string.Format(". {0}", chatRes.Output);
                            }
                            catch(Exception)
                            { }
                        }
                        return outputString;
                    }
                    else
                        return "ERROR";
                }
                else
                    return "ERROR";
            }
            catch(Exception)
            {
                return "ERROR";
            }
        }

        public string ParseQuery(string query)
        {
            if (!query.StartsWith("QUERYDONE | OK |"))
            {
                QueryResult = null;
            }

            string res;
            try
            {
                Request chatRequest = new Request(query, ChatUser, ChatBot);
                Result chatRes = ChatBot.Chat(chatRequest);
                res = chatRes.Output;

                if (res.StartsWith(PatternPrepareQuery))
                {   // 1. Odpytanie bazy danych na podstawie znanych informacji
                    // 2. Ponowne zapytanie systemu konwersacyjnego - rekurencja
                    res = ParseQuery(queryDatabase(res, false));
                }
                else if(res.StartsWith(PatternSQLQuery))
                {   // Wywołanie konretnego zapytania SQL
                    res = ParseQuery(queryDatabase(res.Substring(PatternSQLQuery.Length), true));
                }
                else if(res.StartsWith(PatternGetFormBase))
                {
                    res = ParseQuery(getFormBase(res.Substring(PatternGetFormBase.Length)));
                }


            }catch(Exception ex)
            {
                res = "ERROR - Wystąpił błąd podczas przetwarzania zapytania: " + ex.Message;
            }
            return res;
        }


    }
}
