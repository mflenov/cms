namespace FCms.DbContent.Db
{
    internal class MsSqlGenerator : SqlGenerator
    {
        public MsSqlGenerator (string tablename):base(tablename)
        {

        }

        protected override string GetRowLimit(int? top)
        {
            if (top == null) 
                return string.Empty;
            return " TOP " + top.ToString();
        }
    }
}
