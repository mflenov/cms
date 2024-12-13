namespace FCms.DbContent;

public interface IDbConnection
{
    System.Guid Id { get; set; }

    string Name { get; set; }

    string ConnectionString { get; set; }

    FCms.DbContent.DbType DatabaseType { get; set; }
}