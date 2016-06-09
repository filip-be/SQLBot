﻿using AIMLbot;
using Cindalnet.SQLBot.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Model
{
    public class SQLWord
    {
        public enum EWordType
        {
            Value,
            Field,
            Table,
            Unknown
        };


        public string Word { get; set; }
        public string Definition { get; set; }
        public EWordType WordType { get; set; }

        public string SQLColumn { get; set; }
        public string SQLTable { get; set; }

        public SQLWord Parent { get; set; }
        public SQLWord Child { get; set; }
        

        public SQLWord()
        {
            WordType = EWordType.Unknown;
            Word = "";
            Definition = "";
            SQLColumn = null;
            SQLTable = null;
            Parent = null;
            Child = null;
        }

        private string TrimWord(string Word)
        {
            if (Word.EndsWith(".") || Word.EndsWith("?") || Word.EndsWith("!"))
                Word = Word.Substring(0, Word.Length - 1);
            return Word.Trim();
        }

        private string[] findTable(string name)
        {
            List<string> res = new List<string>();
            try
            {
                BazaRelacyjnaDataContext dc = new BazaRelacyjnaDataContext();
                SQLBot_Table[] table = dc.SQLBot_Table.Where(w => w.sqlt_Name == name).ToArray();
                foreach (var tab in table)
                {
                    res.Add(tab.sqlt_SQLName);
                }
            }
            catch (Exception)
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
            catch (Exception)
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

        public string AIMLWhatIs(string FieldName, Bot ChatBot, User ChatUser)
        {
            Request chatRequest = new Request(string.Format("SQLBOT WHAT IS {0}", FieldName), ChatUser, ChatBot);
            Result chatRes = ChatBot.Chat(chatRequest);
            return FieldName = TrimWord(chatRes.Output);
        }

        public string AIMLWhatTypeIs(string FieldName, Bot ChatBot, User ChatUser)
        {
            Request chatRequest = new Request(string.Format("SQLBOT WHAT TYPE IS {0}", FieldName), ChatUser, ChatBot);
            Result chatRes = ChatBot.Chat(chatRequest);
            return FieldName = TrimWord(chatRes.Output);
        }

        public void Initialize(Bot ChatBot, User ChatUser, string FieldName)
        {
            this.Word = FieldName;
            SQLWord sqlWord = this;
            SQLWord sqlWordParent = null;

            while (sqlWord.Word != null)
            {
                try
                {
                    string fieldDesc = AIMLWhatIs(sqlWord.Word, ChatBot, ChatUser);
                    string fieldType = AIMLWhatTypeIs(sqlWord.Word, ChatBot, ChatUser);

                    switch (fieldDesc)
                    {
                        case "UNKNOWN":
                            sqlWord.Definition = null;
                            break;
                        default:
                            sqlWord.Definition = fieldDesc;
                            break;
                    };

                    switch (fieldType)
                    {

                        case "TABLE":
                            sqlWord.WordType = EWordType.Table;
                            break;
                        case "FIELD":
                            sqlWord.WordType = EWordType.Field;
                            break;
                        case "VALUE":
                            sqlWord.WordType = EWordType.Value;
                            break;
                        case "UNKNOWN":
                        default:
                            sqlWord.WordType = EWordType.Unknown;
                            break;
                    };

                    string[] sqlTables = null;
                    string[] sqlFields = null;

                    if (IsTableName(sqlWord.Word, out sqlTables))
                        SQLTable = sqlTables[0];

                    if (IsFieldName(sqlWord.Word, out sqlFields, out sqlTables))
                        SQLColumn = sqlFields[0];
                }
                catch(Exception)
                { }
                finally
                {
                    sqlWord.Parent = sqlWordParent;
                    sqlWordParent = sqlWord;
                    sqlWord = new SQLWord();
                    sqlWord.Word = sqlWordParent.Definition;

                    sqlWordParent.Child = sqlWord;
                }
            };

            sqlWordParent.Child = null;
        }

        public bool isValidTable()
        {
            return SQLTable != null 
                && (WordType == EWordType.Table || WordType == EWordType.Unknown);
        }

        private bool hasValidParentTable()
        {
            SQLWord sqlWord = this;
            while(sqlWord != null && sqlWord.Parent != null)
            {
                if (sqlWord.isValidTable())
                {
                    this.SQLTable = sqlWord.SQLTable;
                    return true;
                }
                else
                    sqlWord = sqlWord.Parent;
            };
            return false;
        }

        public bool isValidColumn()
        {
            return SQLColumn != null
                && (WordType == EWordType.Field || WordType == EWordType.Unknown);
                //&& (SQLTable != null || hasValidParentTable());
        }

        public bool hasValidParentColumn()
        {
            SQLWord sqlWord = this;
            while (sqlWord != null && sqlWord.Parent != null)
            {
                if (sqlWord.isValidColumn())
                {
                    this.SQLColumn = sqlWord.SQLColumn;
                    return true;
                }
                else
                    sqlWord = sqlWord.Parent;
            };
            return false;
        }

        public bool isValidValue()
        {
            return (WordType == EWordType.Value || WordType == EWordType.Unknown)
                && ((SQLTable != null && SQLColumn != null) || hasValidParentColumn());
        }

        public bool isValidWord()
        {
            return isValidTable() || isValidColumn() || isValidValue();
        }

        public enum MissingParameter
        {
            None,
            FieldName,
            TableName
        };

        public bool isValidWord(out MissingParameter mPar)
        {
            mPar = MissingParameter.None;
            return false;
        }
    }
}
