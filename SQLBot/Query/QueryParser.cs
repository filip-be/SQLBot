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
using System.Text.RegularExpressions;

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
                SQLBot_Field[] fields = dc.SQLBot_Field.ToArray();
                SQLBot_Table[] tables = dc.SQLBot_Table.ToArray();
                SQLBot_Function[] functions = dc.SQLBot_Function.ToArray();

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

                foreach(var function in functions)
                {
                    ChatBot.Chat(string.Format("SQLBOT LEARN WHAT IS {0} | {1} | FUNCTION", function.sqlfn_Name, function.sqlfn_Description),
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

        private string[] findConnectedTables(string tableName)
        {
            try
            {
                BazaRelacyjnaDataContext dc = new BazaRelacyjnaDataContext();
                vSQLBotTableJoins[] tableJoins = dc.vSQLBotTableJoins.Where(tj =>
                    tj.Table1NameSQL == tableName || tj.Table2NameSQL == tableName).ToArray();

                string[] tableNames = new string[tableJoins.Length + 1];
                tableNames[0] = tableName;

                for(int tNum = 0; tNum < tableJoins.Length; tNum++)
                {
                    vSQLBotTableJoins tableJoin = tableJoins[tNum];
                    if (tableJoin.Table1NameSQL == tableName)
                        tableNames[tNum + 1] = tableJoin.Table2NameSQL;
                    else
                        tableNames[tNum + 1] = tableJoin.Table1NameSQL;
                }

                return tableNames;
            }
            catch (Exception)
            {
                return new string[] { tableName };
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

        private bool IsValidRecursiveJoin(string tableSource, string tableCurrent, string tableDest, int recursionDepth, out string[] Joins)
        {
            if(recursionDepth < 1)
            {
                Joins = null;
                return false;
            }
            else
            {
                List<string> newJoins = new List<string>();
                bool res = false;
                foreach (var table in findConnectedTables(tableSource))
                {
                    string currentJoin;
                    if (table != tableSource 
                        && table != tableCurrent
                        && IsValidJoin(tableSource, table, out currentJoin))
                    {
                        string JoinString;
                        string[] temporaryJoins;
                        if (IsValidJoin(table, tableDest, out JoinString))
                        {
                            newJoins.Add(
                                string.Format("{0} ON {1}", table, currentJoin));
                            newJoins.Add(
                                string.Format("{0} ON {1}", tableDest, JoinString));
                            res = true;
                            break;
                        }
                        else if (IsValidRecursiveJoin(tableSource, table, tableDest, recursionDepth - 1, out temporaryJoins))
                        {
                            newJoins.Add(
                                string.Format("{0} ON {1}", table, currentJoin));
                            newJoins.AddRange(temporaryJoins);
                            res = true;
                            break;
                        }
                    }
                }
                Joins = newJoins.ToArray();
                return res;
            }
            
        }
        
        private SQLWord checkIfBelongsToTable(string fieldName, string TableName)
        {
            try
            {
                string[] tableNames = findConnectedTables(TableName);
                foreach (var tableName in tableNames)
                {
                    BazaRelacyjnaDataContext dc = new BazaRelacyjnaDataContext();
                    SQLBot_Table table = dc.SQLBot_Table.Where(tab => tab.sqlt_Name == tableName
                        || tab.sqlt_SQLName == tableName).FirstOrDefault();
                    if (table != null)
                    {
                        int tableID = table.sqlt_ID;
                        List<SQLBot_TableDefault> lDefaults = dc.SQLBot_TableDefault.Where(td => td.SQLBot_Field.sqlf_Table == tableID).ToList();
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
            }
            catch(Exception)
            {

            }

            return null;
        }

        private List<SQLWord> InterpretQuery(string query)
        {
            List<SQLWord> sqlWords = new List<SQLWord>();
            List<string> tables = new List<string>();

            QueryInterpreter queryInterp = new QueryInterpreter(query);
            string Table = string.Empty;
            if (queryInterp.IsInterpreted)
            {
                List<Tuple<int, SQLWord>> unknownWords = new List<Tuple<int, SQLWord>>();

                for (int wordNum = 0; wordNum < queryInterp.Words.Count; wordNum++)
                {
                    Word word = queryInterp.Words[wordNum];

                    string FieldName = word.FormBase;
                    SQLWord sqlWord = new SQLWord();
                    sqlWord.Initialize(ChatBot, ChatUser, FieldName);
                    bool isKnown = sqlWord.isValidWord();

                    if (sqlWord.Word == ChatBot.IgnoredItemValue)
                        continue;

                    if (word.PartOfSpeech == Word.SpeechPart.Noun 
                        || word.PartOfSpeech == Word.SpeechPart.Other 
                        || isKnown 
                        || word.PartOfSpeech == Word.SpeechPart.Adjective
                        || word.PartOfSpeech == Word.SpeechPart.Numeral)
                    {
                        Tuple<int, SQLWord> unknownWord;

                        if (unknownWords.Count > 0
                            && unknownWords.Last().Item1 + 1 == wordNum)
                        {
                            unknownWord = unknownWords.Last();
                            SQLWord sqlWordConcat = new SQLWord();
                            sqlWordConcat.Initialize(ChatBot, ChatUser, unknownWord.Item2.Word + " " + FieldName);
                            unknownWord = new Tuple<int, SQLWord>(wordNum, sqlWordConcat);

                            if (sqlWordConcat.isValidWord())
                            {
                                sqlWords.RemoveAt(sqlWords.Count - 1);

                                unknownWords.RemoveAt(unknownWords.Count - 1);
                                sqlWord = sqlWordConcat;
                                isKnown = true;
                            }
                            else if (!isKnown)
                            {
                                unknownWords.RemoveAt(unknownWords.Count - 1);

                                sqlWords.RemoveAt(sqlWords.Count - 1);
                                //wyświetl towary wyprodukowane przez optyka zoo
                            }
                        }
                        else
                        {
                            unknownWord = new Tuple<int, SQLWord>(wordNum, sqlWord);
                        }

                        if (!isKnown)
                        {
                            SQLWord inTableValue = null;
                            foreach (var tableName in tables)
                            {
                                inTableValue = checkIfBelongsToTable(unknownWord.Item2.Word, tableName);
                                if (inTableValue != null)
                                    break;
                            }

                            if (inTableValue == null)
                            {
                                // Spróbuj wykonać to samo dla elementów w ich oryginalnych formach
                                int wordCount = unknownWord.Item2.Word.Split(' ').Length;
                                string originalPhrase = string.Empty;
                                for (int wordNumber = unknownWord.Item1; wordNumber > unknownWord.Item1 - wordCount; wordNumber--)
                                {
                                    originalPhrase = string.Format("{0} {1}", queryInterp.Words[wordNumber].Form, originalPhrase);
                                }
                                inTableValue = checkIfBelongsToTable(originalPhrase, Table);

                                if (inTableValue == null)
                                {
                                    sqlWord = unknownWord.Item2;
                                    unknownWords.Add(unknownWord);
                                }
                            }

                            if (inTableValue != null)
                            {
                                sqlWord = inTableValue;
                                isKnown = sqlWord.isValidWord();
                            }
                        }
                        else if(sqlWord.SQLTable != null)
                        {
                            tables.Add(sqlWord.SQLTable);
                            tables = tables.Distinct().ToList();
                        }

                        sqlWords.Add(sqlWord);
                    }
                    else
                    {
                        Console.WriteLine(string.Format("{0}: {1}", word.PartOfSpeech, word.FormBase));
                        int x = 0;
                        x = x + 1;
                    }
                }
            }

            return sqlWords;
        }

        private static string COLUMNNAME = "COLUMNNAME";

        private SQLBot_Field getColumn(SQLWord word, string TypeName)
        {
            SQLBot_Field field = null;
            if (word.WordType == SQLWord.EWordType.Field)
            {
                field = new BazaRelacyjnaDataContext().
                   SQLBot_Field.Where(f => f.SQLBot_FieldType.sqlft_Name == TypeName
                    && f.sqlf_SQLColumnName == word.SQLColumn).FirstOrDefault();
            }
            else if (word.WordType == SQLWord.EWordType.Table)
            {
                field = new BazaRelacyjnaDataContext().
                   SQLBot_Field.Where(f => f.SQLBot_FieldType.sqlft_Name == TypeName
                    && f.SQLBot_Table.sqlt_SQLName == word.SQLTable).FirstOrDefault();
            }else if (word.WordType == SQLWord.EWordType.Function)
            {
                
            }

            return field;
        }

        private bool getColumnName(SQLWord word, SQLFunction.ColumnType requiredType, out string columnName, out string tableName)
        {
            columnName = string.Empty;
            tableName = string.Empty;

            if (word == null)
                return false;

            SQLBot_Field field = null;
            try
            {   
                switch (requiredType)
                {
                    case SQLFunction.ColumnType.Number:
                        if (word.WordType == SQLWord.EWordType.Number)
                        {   // number

                            return true;
                        }
                        else
                        {
                            field = getColumn(word, "Number");
                        }
                        break;
                    case SQLFunction.ColumnType.Date:
                        break;
                    case SQLFunction.ColumnType.String:
                        break;
                    case SQLFunction.ColumnType.Any:
                        break;
                    case SQLFunction.ColumnType.ID:
                        field = getColumn(word, "ID");
                        break;
                    case SQLFunction.ColumnType.None:
                        return true;
                    default:
                        break;
                };
            }
            catch(Exception)
            {
            }

            if (field != null)
            {
                columnName = field.sqlf_SQLColumnName;
                tableName = field.SQLBot_Table.sqlt_SQLName;
                return true;
            }

            return false;
        }

        private string AnalyzeFunctions(ref List<SQLWord> words)
        {
            string error = string.Empty;
            for(int num = 0; num < words.Count;)
            {
                if (words[num].WordType == SQLWord.EWordType.Function)
                {   // Obsługa funkcji
                    if (words[num].SQLFunction != null)
                    {
                        SQLFunction function = words[num].SQLFunction;
                        string sqlQuery = function.SQLQuery;
                        
                        SQLWord wordBefore = null;
                        SQLWord wordAfter = null;
                        if (num > 0)
                            wordBefore = words[num - 1];
                        if ((num + 1) < words.Count)
                            wordAfter = words[num + 1];

                        string sqlColumn1 = string.Empty;
                        string sqlColumn2 = string.Empty;
                        string sqlTableName1 = string.Empty;
                        string sqlTableName2 = string.Empty;

                        switch(function.columnLocation)
                        {
                            case SQLFunction.ColumnLocation.BEFORE:
                                if (!getColumnName(wordBefore, function.requiredColumnType, out sqlColumn1, out sqlTableName1))
                                {
                                    return "ERROR - INVALID BEFORE PARAMETER!";
                                }
                                else
                                {
                                    sqlQuery = sqlQuery.Replace(COLUMNNAME, sqlColumn1);
                                    words.Remove(wordBefore);
                                    num--;
                                }
                                break;
                            case SQLFunction.ColumnLocation.AFTER:
                                if (!getColumnName(wordAfter, function.requiredColumnType, out sqlColumn1, out sqlTableName1))
                                {
                                    return "ERROR - INVALID AFTER PARAMETER!";
                                }
                                else
                                {
                                    sqlQuery = sqlQuery.Replace(COLUMNNAME, sqlColumn1);
                                    words.Remove(wordAfter);
                                }
                                break;
                            case SQLFunction.ColumnLocation.BOTH:
                                if (!getColumnName(wordBefore, function.requiredColumnType, out sqlColumn1, out sqlTableName1))
                                {
                                    return "ERROR - INVALID BEFORE PARAMETER!";
                                }
                                else if (!getColumnName(wordAfter, function.requiredColumnType, out sqlColumn2, out sqlTableName2))
                                {
                                    return "ERROR - INVALID AFTER PARAMETER!";
                                }
                                else
                                {
                                    var regex = new Regex(Regex.Escape(COLUMNNAME));
                                    sqlQuery = regex.Replace(sqlQuery, sqlColumn1, 1);
                                    sqlQuery = regex.Replace(sqlQuery, sqlColumn2, 1);
                                    words.Remove(wordBefore);
                                    words.Remove(wordAfter);
                                    num--;
                                }
                                break;
                            case SQLFunction.ColumnLocation.NONE:
                                sqlQuery = sqlQuery.Replace(COLUMNNAME, "");
                                break;
                        };

                        words[num].Word = string.Format("{0} as \"{1}\"", sqlQuery, words[num].Word);
                        words[num].WordType = SQLWord.EWordType.SQL;
                        if (sqlTableName1 != null)
                        {
                            words[num].SQLTable = sqlTableName1;
                            if(sqlTableName2 != string.Empty && sqlTableName1 != sqlTableName2)
                            {
                                SQLWord newTable = new SQLWord();
                                newTable.WordType = SQLWord.EWordType.Table;
                                newTable.SQLTable = sqlTableName2;
                                words.Add(newTable);
                            }
                        }


                        SQLWord currentWord = words[num];
                        switch(function.functionLocation)
                        {
                            case SQLFunction.FunctionLocation.FRONT:
                                words.RemoveAt(num);
                                words.Insert(0, currentWord);
                                break;
                            case SQLFunction.FunctionLocation.END:
                                words.RemoveAt(num);
                                words.Add(currentWord);
                                break;
                            case SQLFunction.FunctionLocation.INLINE:
                                break;
                            default: 
                                break;
                        };
                    }
                    else
                    {
                        return "ERROR - INVALID FUNCTION";
                    }
                    num++;
                }
                else
                {
                    num++;
                }
            }
            return error;
        }

        private string AnalyzeNumbers(ref List<SQLWord> words)
        {
            string error = string.Empty;

            for(int num = 0; num < words.Count;)
            {
                if (words[num].WordType == SQLWord.EWordType.Number)
                    ;
                num++;
            }

            return error;
        }

        private string prepareQuery(string chatResponse)
        {
            string res = "";

            var parameters = TrimWord(chatResponse).Split('|');
            if (parameters.Length > 1)
            {
                List<Tuple<string, string>> wordsToPush = new List<Tuple<string, string>>();

                try
                {   // Wyczyść stosy
                    foreach(var fieldName in new string[]{"FIELD", "VALUE", "TABLE", "UNKNOWN", "JOIN", "GROUPBY"})
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
                    {   // Zinterpretuj wypowiedź
                        List<SQLWord> words = InterpretQuery(parameters[argsNum]);

                        string err = AnalyzeNumbers(ref words);
                        err = AnalyzeFunctions(ref words);

                        if (err.Length > 0)
                        {
                            res = string.Format("ERROR - {0}", err);
                        }
                        else
                        {
                            string Table = string.Empty;
                            foreach (var sqlWord in words)
                            {
                                if (sqlWord.isValidWord())
                                {
                                    if (sqlWord.isValidColumn())
                                    {
                                        wordsToPush.Add(new Tuple<string, string>("FIELD", 
                                            sqlWord.SQLColumn));
                                    }
                                    else if (sqlWord.isValidValue())
                                    {
                                        wordsToPush.Add(
                                            new Tuple<string, string>(
                                                "VALUE",
                                                string.Format("{0}='{1}'", 
                                                    sqlWord.SQLColumn, sqlWord.Word)));
                                    } else if(sqlWord.WordType == SQLWord.EWordType.SQL)
                                    {
                                        wordsToPush.Add(new Tuple<string, string>(
                                                "FIELD",
                                                sqlWord.Word));
                                    }


                                    if (sqlWord.SQLTable != null)
                                    {
                                        if (Table.Length == 0)
                                        {
                                            wordsToPush.Add(new Tuple<string, string>("TABLE", sqlWord.SQLTable));
                                            Table = sqlWord.SQLTable;
                                        }
                                        else if (Table != sqlWord.SQLTable)
                                        {
                                            string JoinString;
                                            string[] JoinStrings;
                                            if (IsValidJoin(Table, sqlWord.SQLTable, out JoinString))
                                            {
                                                wordsToPush.Add(
                                                    new Tuple<string, string>("JOIN",
                                                        string.Format("{0} ON {1}", sqlWord.SQLTable, JoinString)));
                                            }
                                            else
                                            {   // Nieprawidłowe złączenie - być może jest wielokrotne złączenie
                                                if (IsValidRecursiveJoin(Table, Table, sqlWord.SQLTable, 5, out JoinStrings))
                                                {
                                                    foreach (var join in JoinStrings)
                                                    {
                                                        wordsToPush.Add(
                                                            new Tuple<string, string>("JOIN", join));
                                                    }
                                                }
                                                else
                                                {
                                                    int x = 0;
                                                    x = x + 1;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (sqlWord.Definition != ChatBot.IgnoredItemValue)
                                        wordsToPush.Add(new Tuple<string, string>("UNKNOWN",
                                            string.Format("{0} MISSING {1}", sqlWord.Word, sqlWord.MissingObject())));
                                }
                            }

                            foreach(var word in words)
                            {
                                if(word.WordType == SQLWord.EWordType.SQL 
                                    && word.SQLFunction != null
                                    && word.SQLFunction.GroupByRequired)
                                {
                                    foreach (var field in words)
                                    {
                                        if (field.isValidColumn())
                                        {
                                            Tuple<string, string> groupByTuple =
                                                new Tuple<string, string>("GROUPBY", field.SQLColumn);
                                            if (!wordsToPush.Contains(groupByTuple))
                                                wordsToPush.Add(groupByTuple);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    res = "ERROR - PARSING PARAMETERS";
                }

                wordsToPush = wordsToPush.Distinct().ToList();
                wordsToPush.Reverse();
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

        private string QueryException { get; set; }

        private bool executeQuery(string SQLQuery)
        {
            bool res = false;
            try
            {
                QueryException = string.Empty;
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
            catch(Exception ex)
            {
                QueryException = ex.Message;
            }
            return res;
        }

        private string queryDatabase(string chatResponse, bool isCleanSQLQuery)
        {
            string res = chatResponse;

            string SQLQuery;
            if (isCleanSQLQuery)
            {
                SQLQuery = TrimWord(chatResponse);

                bool QueryResult = executeQuery(SQLQuery);

                res = string.Format("QUERYDONE | {0} | {1} | {2}",
                                        QueryResult ? "OK" : "ERROR",
                                        SQLQuery,
                                        !QueryResult && QueryException.Length > 0 ? QueryException : "NONE");
            }
            else
            {
                SQLQuery = prepareQuery(chatResponse);
                res = SQLQuery;
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
