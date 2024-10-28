    using System;
    
    namespace FCms.Content;
    
    public interface IDbConnection
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string ConnectionString { get; set; }

        FCms.DbContent.DbType DatabaseType { get; set; }
    }