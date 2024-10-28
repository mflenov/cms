using System;
using System.Linq;
using System.Collections.Generic;
using FCms.Content;

namespace FCmsManagerAngular.ViewModels;

public class DbConnectionViewModel
{
    public DbConnectionViewModel()  {
    }

    public DbConnectionViewModel(IDbConnection dbConnection)  {
        Id = dbConnection.Id;
        Name = dbConnection.Name;
        ConnectionString = dbConnection.ConnectionString;
        DatabaseType = dbConnection.DatabaseType.ToString();
    }

    public Guid? Id { get; set; }

    public string Name { get; set; }

    public string ConnectionString { get; set; }

    public string DatabaseType { get; set; }

    public void MapToModel(IDbConnection model)
    {
        model.Id = this.Id ?? Guid.NewGuid();
        model.Name = this.Name;
        model.ConnectionString = this.ConnectionString;
        
        FCms.DbContent.DbType databaseType;
        if (Enum.TryParse(this.DatabaseType, out databaseType)) {
            model.DatabaseType = databaseType;
        }

    }
}