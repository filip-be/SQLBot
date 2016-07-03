using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cindalnet.SQLBot.Model
{
    public class SQLFunction
    {
        public enum FunctionLocation
        {
            FRONT,
            SELECT,
            WHERE,
            END
        };

        public enum ColumnLocation
        {
            BEFORE,
            AFTER,
            BOTH,
            NONE,
            ANY
        };

        public enum ColumnType
        {
            Date,
            String,
            Number,
            None,
            Any,
            ID
        }

        public FunctionLocation functionLocation { get; set; }
        public ColumnLocation columnLocation { get; set; }
        public ColumnType requiredColumnType { get; set; }
        public string Name { get; set; }
        public string SQLQuery { get; set; }
        public string Description { get; set; }
        public bool GroupByRequired { get; set; }

        public static string LessThanMark = "LESS_THAN_CHAR";

        private void setFunctionLocation(string location)
        {
            /*
             * FRONT
             * SELECT
             * WHERE
             * END
             */
            switch (location)
            {
                case "FRONT":
                    functionLocation = FunctionLocation.FRONT;
                    break;
                case "SELECT":
                    functionLocation = FunctionLocation.SELECT;
                    break;
                case "WHERE":
                    functionLocation = FunctionLocation.WHERE;
                    break;
                case "END":
                    functionLocation = FunctionLocation.END;
                    break;
                default:
                    functionLocation = FunctionLocation.SELECT;
                    break;
            };
        }

        private void setColumnLocation(string location)
        {
            /*
                BEFORE
                AFTER
                BOTH
                NONE
                ANY
            */
            switch (location)
            {
                case "BEFORE":
                    columnLocation = ColumnLocation.BEFORE;
                    break;
                case "AFTER":
                    columnLocation = ColumnLocation.AFTER;
                    break;
                case "BOTH":
                    columnLocation = ColumnLocation.BOTH;
                    break;
                case "NONE":
                    columnLocation = ColumnLocation.NONE;
                    break;
                case "ANY":
                    columnLocation = ColumnLocation.ANY;
                    break;
                default:
                    columnLocation = ColumnLocation.NONE;
                    break;
            };
        }

        private void setRequiredWordType(string wordType)
        {
            /*
                Date
                String
                Number
                None
                Any
                ID
            */
            switch(wordType)
            {
                case "Date":
                    requiredColumnType = ColumnType.Date;
                    break;
                case "String":
                    requiredColumnType = ColumnType.String;
                    break;
                case "Number":
                    requiredColumnType = ColumnType.Number;
                    break;
                case "None":
                    requiredColumnType = ColumnType.None;
                    break;
                case "Any":
                    requiredColumnType = ColumnType.Any;
                    break;
                case "ID":
                    requiredColumnType = ColumnType.ID;
                    break;
                default:
                    requiredColumnType = ColumnType.None;;
                    break;
            }
        }

        public SQLFunction(SQLBot.Database.SQLBot_Function function)
        {
            setFunctionLocation(function.SQLBot_FunctionLocation.sqlfl_Name);
            setColumnLocation(function.SQLBot_FunctionColumnLocation.sqlfncl_Name);
            setRequiredWordType(function.SQLBot_FieldType.sqlft_Name);

            this.Name = function.sqlfn_Name;
            this.SQLQuery = function.sqlfn_SQL.Replace("<", LessThanMark);
            this.Description = function.sqlfn_Description;
            this.GroupByRequired = function.sqlfn_RequireGroupBy;
        }
    }
}
