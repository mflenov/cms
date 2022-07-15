using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using FCms.DbContent.Models;

namespace FCms.DbContent.Db
{
    internal abstract class SqlGenerator
    {
        private string tablename;

        public SqlGenerator(string tableName)
        {
            this.tablename = tableName;
        }

        public SqlQueryModel GetSearchQuery(ContentSearchModel model)
        {
            if (model == null)
                model = new ContentSearchModel();
            
            var where = GetWhereClause(model);

            var select = String.Join(",", model.Columns);
            if (String.IsNullOrEmpty(select))
                select = "*";

            return new SqlQueryModel(
                String.Format(GetSqlTemplage(), GetRowLimit(model.Top), select, tablename, where.Sql),
                where.Parameters
            );
        }

        protected SqlQueryModel GetWhereClause(ContentSearchModel model)
        {
            SqlQueryModel result = new SqlQueryModel();
            if (model.Filters == null || model.Filters.Count == 0)
            {
                result.Sql = "1 = 1";
                return result;
            }

            StringBuilder where = new StringBuilder();
            int index = 0;
            foreach (var filter in model.Filters)
            {
                if (where.Length > 0) 
                    where.Append(" AND ");
                where.Append($" { DbHelpers.SanitizeDbName(filter.Column.Name) } = @f{index} ");

                var parameter = new SqlParameter("f" + index.ToString(), filter.Column.GetSqlDbTypeName());
                parameter.Value = filter.Value;
                result.Parameters.Add(parameter);
                index++;
            }
            return result;
        }

        protected abstract string GetRowLimit(int? top);

        protected virtual string GetSqlTemplage()
        {
            return @"
                SELECT {0} {1}
                FROM {2}
                WHERE {3}
            ";
        }
    }
}
