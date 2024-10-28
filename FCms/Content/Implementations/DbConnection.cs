using System;
using System.Collections.Generic;

namespace FCms.Content;

public class DbConnection: IDbConnection {
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string ConnectionString { get; set; }

    public FCms.DbContent.DbType DatabaseType { get; set; }
}