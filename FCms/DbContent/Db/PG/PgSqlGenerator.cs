namespace FCms.DbContent.Db
{
    internal class PgSqlGenerator : SqlGenerator
    {
        public PgSqlGenerator (string tablename):base(tablename)
        {

        }

        protected override string GetRowLimit(int? top)
        {
            if (top == null) 
                return string.Empty;
            return " limit " + top.ToString();
        }

        protected override string GetDbTableName()
        {
            return "\"" + base.GetDbTableName() + "\"";
        }

        protected override string GetSqlTemplate()
        {
            return @"
                SELECT {1} 
                FROM {2}
                WHERE {3}
                {0}
            ";
        }

    }
}
