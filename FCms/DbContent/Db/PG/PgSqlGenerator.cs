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

        protected override string GetSqlTemplage()
        {
            return @"
                SELECT {0} 
                FROM {2}
                WHERE {3}
                {1}
            ";
        }

    }
}
