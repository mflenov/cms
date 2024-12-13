namespace FCms.DbContent;

public class DbConnection: IDbConnection {
    public System.Guid Id { get; set; }

    public string Name { get; set; }

    public string ConnectionString { get; set; }

    public FCms.DbContent.DbType DatabaseType { get; set; }
}