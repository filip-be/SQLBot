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

                ChatBot.Chat("SQLBOT AIML LEARN POLISH NUMBERS",
                        ChatUser.UserID);
                ChatBot.Chat("SQLBOT AIML LEARN POLISH DATES",
                        ChatUser.UserID);

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

                    if (sqlWord.WordType == SQLWord.EWordType.Number)
                    {
                        if (sqlWords.Count > 0
                            && sqlWords.Last().WordType == SQLWord.EWordType.Number
                            && sqlWords.Last().Word.EndsWith(queryInterp.Words[wordNum - 1].FormBase))
                        {
                            sqlWords.Last().Word += string.Format(" {0}", sqlWord.Word);
                            switch (sqlWord.CharBeforeNumber)
                            {
                                case SQLWord.ECharBeforeNumber.Add:
                                    sqlWords.Last().Number += sqlWord.Number;
                                    break;
                                case SQLWord.ECharBeforeNumber.Multiply:
                                    sqlWords.Last().Number *= sqlWord.Number;
                                    break;
                                case SQLWord.ECharBeforeNumber.Divide:
                                    sqlWords.Last().Number /= sqlWord.Number;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            sqlWords.Add(sqlWord);
                        }
                        continue;
                    }

                    if (word.PartOfSpeech == Word.SpeechPart.Noun 
                        || word.PartOfSpeech == Word.SpeechPart.Other 
                        || isKnown 
                        || word.PartOfSpeech == Word.SpeechPart.Adjective)
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
                            }
                        }
                        else
                        {
                            if (sqlWords.Count() > 0)
                            {
                                SQLWord sqlWordConcat = new SQLWord();
                                sqlWordConcat.Initialize(ChatBot, ChatUser, sqlWords.Last().Word + " " + FieldName);
                                unknownWord = new Tuple<int, SQLWord>(wordNum, sqlWordConcat);

                                if (sqlWordConcat.isValidWord())
                                {
                                    sqlWords.RemoveAt(sqlWords.Count - 1);
                                    sqlWord = sqlWordConcat;
                                    isKnown = true;
                                }
                                else if (!isKnown)
                                {
                                    unknownWord = new Tuple<int, SQLWord>(wordNum, sqlWord);
                                }
                            }
                            else
                            {
                                unknownWord = new Tuple<int, SQLWord>(wordNum, sqlWord);
                            }
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
                string sqlColumn = word.SQLColumn;
                if (word.SQLTable != null && sqlColumn.StartsWith(word.SQLTable + "."))
                    sqlColumn = sqlColumn.Substring(word.SQLTable.Length + 1);
                field = new BazaRelacyjnaDataContext().
                   SQLBot_Field.Where(f => f.SQLBot_FieldType.sqlft_Name == TypeName
                    && f.sqlf_SQLColumnName == sqlColumn).FirstOrDefault();
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
                        {   // Number
                            columnName = string.Format("{0}", word.Number);
                            return true;
                        }
                        else
                        {
                            field = getColumn(word, "Number");
                        }
                        break;
                    case SQLFunction.ColumnType.Date:
                        if(word.WordType == SQLWord.EWordType.Date)
                        {
                            columnName = "UNKNOWN";
                            return true;
                        }
                        else
                        {
                            field = getColumn(word, "Date");
                        }
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
                tableName = field.SQLBot_Table.sqlt_SQLName;
                columnName = field.sqlf_SQLColumnName;
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
                                    return string.Format("Nieprawidłowy parametr funkcji - '{0}' powinien mieć wartość {1}", wordBefore.Word, function.requiredColumnType);
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
                                    return string.Format("Nieprawidłowy parametr funkcji - '{0}' powinien mieć wartość {1}", wordAfter.Word, function.requiredColumnType);
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
                                    return string.Format("Nieprawidłowy parametr funkcji - '{0}' powinien mieć wartość {1}", wordBefore.Word, function.requiredColumnType);
                                }
                                else if (!getColumnName(wordAfter, function.requiredColumnType, out sqlColumn2, out sqlTableName2))
                                {
                                    return string.Format("Nieprawidłowy parametr funkcji - '{0}' powinien mieć wartość {1}", wordAfter.Word, function.requiredColumnType);
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
                            case SQLFunction.ColumnLocation.ANY:
                                if (getColumnName(wordBefore, function.requiredColumnType, out sqlColumn1, out sqlTableName1))
                                {
                                    var regex = new Regex(Regex.Escape(COLUMNNAME));
                                    sqlQuery = regex.Replace(sqlQuery, sqlColumn1, 1);
                                    words.Remove(wordBefore);
                                    num--;
                                }
                                else if (getColumnName(wordAfter, function.requiredColumnType, out sqlColumn1, out sqlTableName1))
                                {
                                    var regex = new Regex(Regex.Escape(COLUMNNAME));
                                    sqlQuery = regex.Replace(sqlQuery, sqlColumn1, 1);
                                    words.Remove(wordAfter);
                                }
                                else
                                {
                                    return string.Format("Brak prawidłowego parametru funkcji '{0}' - poszukiwana wartość typu {1}", function.Name, function.requiredColumnType);
                                }
                                break;
                            default:
                                break;
                        };

                        if (function.functionLocation == SQLFunction.FunctionLocation.SELECT)
                            words[num].Word = string.Format("{0} as \"{1}\"", sqlQuery, words[num].Word);
                        else
                            words[num].Word = sqlQuery;
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
                            case SQLFunction.FunctionLocation.SELECT:
                                words.RemoveAt(num);
                                words.Add(currentWord);
                                break;
                            case SQLFunction.FunctionLocation.WHERE:
                                words.RemoveAt(num);
                                if(words.Count > 0 
                                    && words.Last().isFunction() 
                                    && words.Last().SQLFunction.functionLocation == SQLFunction.FunctionLocation.END)
                                    words.Insert(words.Count() - 1, currentWord);
                                else
                                    words.Add(currentWord);
                                break;
                            case SQLFunction.FunctionLocation.END:
                                words.RemoveAt(num);
                                words.Add(currentWord);
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
                if (words[num].WordType == SQLWord.EWordType.Number
                    && words.Count > 0
                    && num > 0
                    && words[num - 1].WordType == SQLWord.EWordType.Number)
                {
                    int x = 0;
                    x = x + 1;
                }
                num++;
            }

            return error;
        }

        private string AnalyzeDates(ref List<SQLWord> words)
        {
            string error = string.Empty;

            for (int num = 0; num < words.Count; )
            {
                bool increment = true;
                if(words[num].WordType == SQLWord.EWordType.Date)
                {
                    if(num > 0
                       && words[num-1].WordType == SQLWord.EWordType.Number)
                    {
                        string[] fields = words[num].Definition.Split(' ');
                        double multiplier;

                        if(fields.Length == 2
                            && double.TryParse(fields[0], out multiplier))
                        {
                            multiplier *= words[num - 1].Number;
                            words[num].Definition = string.Format("{0} {1}", multiplier, fields[1]);
                            words.RemoveAt(num - 1);
                            increment = false;
                        }
                    }
                }
                else if(words[num].WordType == SQLWord.EWordType.Month)
                {
                    if(num > 0
                        && words[num-1].WordType == SQLWord.EWordType.Number)
                    {   // dzień miesiąca
                        words[num].Word = string.Format("{0} {1}", words[num - 1].Number, words[num].Word);
                        increment = false;
                    }

                    if (num < (words.Count - 1)
                        && words[num + 1].WordType == SQLWord.EWordType.Number)
                    {   // rok
                        words[num].Word = string.Format("{0} {1}", words[num].Word, words[num + 1].Number);
                        words.RemoveAt(num + 1);
                    }

                    if(!increment)
                    {
                        words.RemoveAt(num - 1);
                    }
                }

                if(increment)
                    num++;
            }

            return error;
        }

        private string GetDatesQuery(string columnName, double multiplier, SQLWord wordDate, string dateOpacity)
        {
            string res = string.Empty;

            string dateOperator = ">=";

            switch(dateOpacity)
            {
                case "FROM":
                    dateOperator = "=";
                    break;
                case "LAST":
                    dateOperator = ">=";
                    break;
                case "BEFORE":
                    dateOperator = SQLFunction.LessThanMark;
                    break;
                case "AFTER":
                    dateOperator = ">=";
                    break;
                default:
                    dateOperator = ">=";
                    break;
            }

            string[] fields = null;
            if (wordDate.Definition != null && wordDate.Definition.Contains(' '))
                fields = wordDate.Definition.Split(' ');
            else if (wordDate.Word != null && wordDate.Word.Contains(' '))
                fields = wordDate.Word.Split(' ');
            else
                fields = new string[0];

            double inDateMultiplier;

            if (fields != null
                &&fields.Length > 1
                && wordDate.WordType == SQLWord.EWordType.Date
                && double.TryParse(fields[0], out inDateMultiplier))
            {
                multiplier *=  inDateMultiplier;
            }

            if(wordDate.WordType == SQLWord.EWordType.Date
                && fields.Length == 2)
            {
                Request chatRequest = new Request(
                        string.Format("SQLBOT AIML DATE QUERY {0} + - {1}", fields[1], multiplier),
                        ChatUser,
                        ChatBot);
                Result chatRes = ChatBot.Chat(chatRequest);

                res = string.Format("{0} {1} {2}", columnName, dateOperator, chatRes.Output);
            }
            else if(wordDate.WordType == SQLWord.EWordType.Month)
            {
                double day = -1;
                double year = -1;
                if (fields.Length > 1)
                {   // dzień miesiąca
                    double.TryParse(fields[0], out day);
                }

                if (fields.Length > 1
                    && !double.TryParse(fields[1], out year)
                    && fields.Length > 2)
                {   // rok
                    double.TryParse(fields[2], out year);
                }

                if (dateOpacity == "LAST" || dateOpacity == "FROM")
                {
                    if (dateOpacity == "LAST")
                        year = DateTime.Now.Year - 1;

                    string SQLQuery = string.Empty;
                    if (day != -1)
                    {
                        SQLQuery += string.Format("DAY({0}) = '{1}' AND ", columnName, day);
                    }

                    SQLQuery += string.Format("MONTH({0}) = '{1}'", columnName, day);

                    if (year != -1)
                    {
                        SQLQuery += string.Format(" AND YEAR({0}) = '{1}'", columnName, year);
                    }
                    res = SQLQuery;
                }
                else
                {
                    if (day == -1)
                        day = 1;
                    if (year == -1)
                        year = DateTime.Now.Year;

                    Request chatRequest = new Request(
                        string.Format("SQLBOT AIML DATE CAST YEAR {0} MONTH {1} DAY {2}", year, wordDate.Number, day),
                        ChatUser,
                        ChatBot);
                    Result chatRes = ChatBot.Chat(chatRequest);

                    res = string.Format("{0} {1} {2}", columnName, dateOperator, chatRes.Output);
                }
            }

            return res;
        }

        private string AnalyzeDateAffixes(ref List<SQLWord> words)
        {
            string error = string.Empty;
            string columnName = string.Empty;
            string tableName = string.Empty;

            foreach(var word in words)
            {
                if(getColumnName(word, SQLFunction.ColumnType.Date, out columnName, out tableName))
                {
                    columnName = string.Format("{0}.{1}", tableName, columnName);
                    break;
                }
            }

            if (columnName == string.Empty)
                return string.Format("Nie udał się znaleźć żadnej kolumny powiązanej z datą");


            if (words.FindIndex(w => w.WordType == SQLWord.EWordType.DateAffix) != -1)
            {   // określenie przedziału
                for (int num = 0; num < words.Count; )
                {
                    if (words[num].WordType == SQLWord.EWordType.DateAffix)
                    {
                        string SQLQuery = string.Empty;

                        double multiplier = 1;
                        if(num > 0 && words[num - 1].WordType == SQLWord.EWordType.Number)
                        {
                            multiplier = words[num - 1].Number;
                            words.RemoveAt(num - 1);
                            num--;
                        }

                        if((num+1) >= words.Count
                            || (words[num + 1].WordType != SQLWord.EWordType.Date && words[num + 1].WordType != SQLWord.EWordType.Month))
                        {
                            return string.Format("Brak opisu poszukiwanego zakresu dat po słowie '{0}'", words[num].Word);
                        }
                        //wyświetl zarobki pracowników z poprzedniego stycznia
                        //wyświetl zarobki pracowników z poprzednich trzech lat
                        //wyświetl zarobki pracowników z trzech poprzednich lat

                        if(words[num].Definition == "BETWEEN")
                        {
                            if ((num + 2) >= words.Count
                            || (words[num + 2].WordType != SQLWord.EWordType.Date && words[num + 1].WordType != SQLWord.EWordType.Month))
                            {
                                return string.Format("Brak opisu poszukiwanego zakresu dat po słowie '{0}'", words[num].Word);
                            }

                            SQLQuery = string.Format("{0} AND {1}",
                                TrimWord(GetDatesQuery(columnName, multiplier, words[num + 1], "AFTER")),
                                TrimWord(GetDatesQuery(columnName, multiplier, words[num + 2], "BEFORE")));

                            words.RemoveAt(num + 1);
                            words.RemoveAt(num + 1);
                        }
                        else
                        {
                            SQLQuery = GetDatesQuery(columnName, multiplier, words[num + 1], words[num].Definition);

                            words.RemoveAt(num + 1);
                        }

                        words[num].SQLColumn = columnName;
                        words[num].Definition = TrimWord(SQLQuery);
                    }

                    num++;
                }
            }
            else if(words.FindIndex(w => w.WordType == SQLWord.EWordType.Date) != -1)
            {   // zakres dat, np. wyświetl ... z 3 lat
                for (int num = 0; num < words.Count; )
                {
                    if (words[num].WordType == SQLWord.EWordType.Date)
                    {
                        words[num].Definition = TrimWord(GetDatesQuery(columnName, 1, words[num], "LAST"));
                        words[num].SQLColumn = columnName;
                        words[num].WordType = SQLWord.EWordType.DateAffix;
                    }

                    num++;
                }
            }
            else if(words.FindIndex(w => w.WordType == SQLWord.EWordType.Month) != -1)
            {   // konkretna data, np. wyświetl ... ze stycznia 2015
                for (int num = 0; num < words.Count; )
                {
                    if (words[num].WordType == SQLWord.EWordType.Month)
                    {
                        words[num].Definition = TrimWord(GetDatesQuery(columnName, 1, words[num], "FROM"));
                        words[num].SQLColumn = columnName;
                        words[num].WordType = SQLWord.EWordType.DateAffix;
                    }

                    num++;
                }
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
                    foreach(var fieldName in new string[]{"BEFORE FIELD", "FIELD", "VALUE", "TABLE", "UNKNOWN", "JOIN", "GROUPBY", "QUERY END"})
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

                        if (err.Length > 0)
                        {
                            return string.Format("ERROR - {0}", err);
                        }

                        err = AnalyzeDates(ref words);

                        if (err.Length > 0)
                        {
                            return string.Format("ERROR - {0}", err);
                        }

                        err = AnalyzeFunctions(ref words);

                        if (err.Length > 0)
                        {
                            return string.Format("ERROR - {0}", err);
                        }

                        err = AnalyzeDateAffixes(ref words);

                        if(err.Length > 0)
                        {
                            return string.Format("ERROR - {0}", err);
                        }


                        if (words.Count > 0
                            && words.Where(w => w.WordType == SQLWord.EWordType.Number).Count() > 0)
                        {   // TOP N ROWS
                            int indexOfNumber = words.IndexOf(words.Where(w => w.WordType == SQLWord.EWordType.Number).First());
                            SQLWord topWord = new SQLWord();
                            topWord.Initialize(ChatBot, ChatUser, "Pierwsze");
                            if (topWord.isFunction())
                            {
                                words.Insert(indexOfNumber + 1, topWord);
                                err = AnalyzeFunctions(ref words);
                                if (err.Length > 0)
                                {
                                    return string.Format("ERROR - {0}", err);
                                }
                            }
                        }

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
                                }
                                else if (sqlWord.WordType == SQLWord.EWordType.SQL)
                                {
                                    if (sqlWord.SQLFunction != null)
                                    {
                                        switch(sqlWord.SQLFunction.functionLocation)
                                        {
                                            case SQLFunction.FunctionLocation.SELECT:
                                                wordsToPush.Add(new Tuple<string, string>(
                                                    "FIELD",
                                                    sqlWord.Word));
                                                break;
                                            case SQLFunction.FunctionLocation.WHERE:
                                                wordsToPush.Add(new Tuple<string, string>(
                                                    "VALUE",
                                                    sqlWord.Word));
                                                break;
                                            case SQLFunction.FunctionLocation.FRONT:
                                                wordsToPush.Add(new Tuple<string, string>(
                                                    "BEFORE FIELD",
                                                    sqlWord.Word));
                                                break;
                                            case SQLFunction.FunctionLocation.END:
                                                wordsToPush.Add(new Tuple<string, string>(
                                                    "QUERY END",
                                                    sqlWord.Word));
                                                break;
                                            default:
                                                wordsToPush.Add(new Tuple<string, string>(
                                                    "FIELD",
                                                    sqlWord.Word));
                                                break;
                                        };
                                    }
                                    else
                                    {
                                        wordsToPush.Add(new Tuple<string, string>(
                                                "FIELD",
                                                sqlWord.Word));
                                    }
                                }
                                else if (sqlWord.WordType == SQLWord.EWordType.DateAffix)
                                {
                                    wordsToPush.Add(new Tuple<string, string>(
                                                "VALUE",
                                                sqlWord.Definition));
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

                        foreach (var word in words)
                        {
                            if (word.WordType == SQLWord.EWordType.SQL
                                && word.SQLFunction != null
                                && word.SQLFunction.GroupByRequired)
                            {
                                foreach (var field in words)
                                {
                                    if (field.isValidColumn() || (field.isFunction() && field.SQLColumn != string.Empty))
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

                    cmd.CommandText = TrimWord(SQLQuery).Replace(SQLFunction.LessThanMark, "<");

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

        private string queryDatabase(string chatResponse)
        {
            string res = chatResponse;

            string SQLQuery;
            SQLQuery = TrimWord(chatResponse);

            bool QueryResult = executeQuery(SQLQuery);

            res = string.Format("QUERYDONE | {0} | {1} | {2}",
                                    QueryResult ? "OK" : "ERROR",
                                    SQLQuery,
                                    !QueryResult && QueryException.Length > 0 ? QueryException : "NONE");

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
                    res = ParseQuery(prepareQuery(res));
                }
                else if(res.StartsWith(PatternSQLQuery))
                {   // Wywołanie konretnego zapytania SQL
                    res = ParseQuery(queryDatabase(res.Substring(PatternSQLQuery.Length)));
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
