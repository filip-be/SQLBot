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
        }

        public void UpdateAIMLKnowledge()
        {
            try
            {
                BazaRelacyjnaDataContext dc = new BazaRelacyjnaDataContext();
                SQLBot_Field[] fields = dc.SQLBot_Field.Take(10).ToArray();
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

        private SQLWord checkIfBelongsToTable(string fieldName, string TableName)
        {
            try
            {
                BazaRelacyjnaDataContext dc = new BazaRelacyjnaDataContext();
                SQLBot_Table table = dc.SQLBot_Table.Where(tab => tab.sqlt_Name == TableName
                    || tab.sqlt_SQLName == TableName).FirstOrDefault();
                if (table != null)
                {
                    int tableID = table.sqlt_ID;
                    List<SQLBot_TableDefault> lDefaults = dc.SQLBot_TableDefault.Where(td => td.sqld_Table == tableID).ToList();
                    foreach (var defaultColumn in lDefaults)
                    {
                        string SQLQuery = string.Format("SELECT TOP 1 'TRUE' FROM {0} WHERE {1}='{2}'", 
                            table.sqlt_SQLName,
                            defaultColumn.SQLBot_Field.sqlf_SQLColumnName,
                            fieldName);
                        if (executeQuery(SQLQuery) && QueryResult.Row.Count > 0 && QueryResult.Row[0][0].Text == "TRUE")
                        {
                            Result res = ChatBot.Chat(string.Format("SQLBOT LEARN WHAT IS {0} | {1} | {2}", 
                                fieldName,
                                defaultColumn.SQLBot_Field.sqlf_ColumnName,
                                "VALUE"),
                                ChatUser.UserID);
                            
                            SQLWord word = new SQLWord();
                            word.Initialize(ChatBot, ChatUser, fieldName);
                            return word;
                        }
                    }
                }
            }
            catch(Exception)
            {

            }

            return null;
        }

        private string prepareQuery(string chatResponse)
        {
            string res = "";

            string Table = "";
            List<SQLWord> LColumns = new List<SQLWord>();
            List<SQLWord> LFrom = new List<SQLWord>();
            List<SQLWord> LJoin = new List<SQLWord>();
            List<SQLWord> LWhere = new List<SQLWord>();

            var parameters = TrimWord(chatResponse).Split('|');
            if (parameters.Length > 1)
            {
                List<Tuple<string, string>> wordsToPush = new List<Tuple<string, string>>();
                // Wyczyść stosy
                try
                {
                    foreach(var fieldName in new string[]{"FIELD", "VALUE", "TABLE", "UNKNOWN", "JOIN"})
                    {
                        Request chatRequest = new Request(
                            string.Format("SQLBOT AIML STACK CLEAR {0}", fieldName),
                            ChatUser,
                            ChatBot);
                        Result chatRes = ChatBot.Chat(chatRequest);
                    }
                }
                catch(Exception)
                {
                }

                try
                {
                    for (int argsNum = 1; argsNum < parameters.Length; argsNum++)
                    {
                        QueryInterpreter queryInterp = new QueryInterpreter(parameters[argsNum]);
                        if (queryInterp.IsInterpreted)
                        {
                            //string field = queryInterp.DesiredParameter;
                            //
                            List<Tuple<int, string, string>> unknownWords = new List<Tuple<int, string, string>>();

                            for(int wordNum = 0; wordNum < queryInterp.Words.Count; wordNum++)
                            {
                                Word word = queryInterp.Words[wordNum];
                                if(word.PartOfSpeech == Word.SpeechPart.Noun)
                                {
                                    string FieldName = word.FormBase;

                                    SQLWord sqlWord = new SQLWord();
                                    sqlWord.Initialize(ChatBot, ChatUser, FieldName);

                                    bool isKnown = sqlWord.isValidWord();

                                    Tuple<int, string, string> unknownWord;

                                    if (unknownWords.Count > 0
                                        && unknownWords.Last().Item1 + 1 == wordNum)
                                    {
                                        unknownWord = unknownWords.Last();
                                        unknownWords.RemoveAt(unknownWords.Count - 1);
                                        SQLWord sqlWordConcat = new SQLWord();
                                        sqlWordConcat.Initialize(ChatBot, ChatUser, unknownWord.Item2 + " " + FieldName);
                                        unknownWord = new Tuple<int, string, string>(wordNum, unknownWord.Item2 + " " + FieldName, sqlWord.MissingObject());

                                        if(sqlWordConcat.isValidWord())
                                        {
                                            sqlWord = sqlWordConcat;
                                            isKnown = true;
                                        }
                                    }
                                    else
                                    {
                                        unknownWord = new Tuple<int, string, string>(wordNum, FieldName, sqlWord.MissingObject());
                                    }
                                    if (!isKnown)
                                    {
                                        SQLWord inTableValue = checkIfBelongsToTable(FieldName, Table);
                                        if (inTableValue == null)
                                            unknownWords.Add(unknownWord);
                                        else
                                        {
                                            sqlWord = inTableValue;
                                            isKnown = sqlWord.isValidWord();
                                        }
                                    }
                                    
                                    if(isKnown)
                                    {
                                        if (sqlWord.isValidColumn())
                                        {
                                            wordsToPush.Insert(0, new Tuple<string, string>("FIELD", sqlWord.SQLColumn));
                                        }
                                        else if (sqlWord.isValidValue())
                                        {
                                            wordsToPush.Insert(0,
                                                new Tuple<string, string>(
                                                    "VALUE",
                                                    string.Format("{0}='{1}'", sqlWord.SQLColumn, sqlWord.Word)));
                                        }


                                        if (sqlWord.SQLTable != null)
                                        {
                                            if (Table.Length == 0)
                                            {
                                                wordsToPush.Insert(0, new Tuple<string, string>("TABLE", sqlWord.SQLTable));
                                                Table = sqlWord.SQLTable;
                                            }
                                            else if (Table != sqlWord.SQLTable)
                                            {
                                                string JoinString;
                                                if (IsValidJoin(Table, sqlWord.SQLTable, out JoinString))
                                                {
                                                    wordsToPush.Insert(0,
                                                        new Tuple<string, string>("JOIN",
                                                            string.Format("{0} ON {1}", sqlWord.SQLTable, JoinString)));
                                                }
                                                else
                                                {   // Nieprawidłowe złączenie
                                                    int x = 0;
                                                    x = x + 1;
                                                }
                                            }
                                        }
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
                                foreach (var unknownWord in unknownWords)
                                {
                                    wordsToPush.Insert(0, new Tuple<string, string>("UNKNOWN", 
                                        string.Format("{0} MISSING {1}", unknownWord.Item2, unknownWord.Item3)));
                                }
                                //throw new QueryExceptionUnknownParameter(unknownWords.First().Item2);
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

                wordsToPush.Sort();
                wordsToPush = wordsToPush.Distinct().ToList();
                foreach(var wordToPush in wordsToPush)
                {
                    Request chatRequest = new Request(
                        string.Format("SQLBOT AIML {0} PUSH {1}", wordToPush.Item1, wordToPush.Item2),
                        ChatUser,
                        ChatBot);
                    Result chatRes = ChatBot.Chat(chatRequest);
                }
            }
            else
            {
                return "ERROR - NO PARAMETERS";
            }

            if(!res.StartsWith("ERROR"))
            {
                res = "SQLBOT AIML BUILD QUERY";
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
                {
                    SQLQuery = TrimWord(chatResponse);

                    bool QueryResult = executeQuery(SQLQuery);

                    res = string.Format("QUERYDONE | {0} | {1}",
                                            QueryResult ? "OK" : "ERROR",
                                            SQLQuery);
                }
                else
                {
                    SQLQuery = prepareQuery(chatResponse);
                    res = SQLQuery;
                }
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
                string res = "";
                string[] orgAndBaseSearch = TrimWord(sentence).Split(new[] { PatternGetFormBase }, StringSplitOptions.None);
                if(orgAndBaseSearch.Length > 0)
                {
                    res = orgAndBaseSearch[0];
                    for(int baseSearchNum = 1; baseSearchNum < orgAndBaseSearch.Length; baseSearchNum++)
                    {
                        string[] sentenceAndResponse = TrimWord(orgAndBaseSearch[baseSearchNum]).Split(new[] { "CSPLITSENTENCE" }, StringSplitOptions.None);
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
                                    catch (Exception)
                                    { }
                                }
                                res += outputString;
                            }
                            else
                                return "ERROR";
                        }
                        else
                            return "ERROR";
                    }
                }
                return res;
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
                else if(res.Contains(PatternGetFormBase))
                {
                    res = ParseQuery(getFormBase(res));
                }


            }catch(Exception ex)
            {
                res = "ERROR - Wystąpił błąd podczas przetwarzania zapytania: " + ex.Message;
            }
            return res;
        }


    }
}
