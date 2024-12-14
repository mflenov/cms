using System;
using System.Linq;
using System.Collections.Generic;
using FCms.Content;
using FCms.DbContent;

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

    public void Add(ICmsManager manager) {
        IDbConnection connectionModel = new DbConnection();
        MapToModel(connectionModel);
        manager.Data.DbConnections.Add(connectionModel);
        manager.Save(); 
    }

    public void Update(ICmsManager manager) {
        int repoindex = manager.Data.DbConnections.Select((v, i) => new { filter = v, Index = i }).FirstOrDefault(x => x.filter.Id == Id)?.Index ?? -1;
        if (repoindex < 0)
        {
            throw new Exception("The filter definition not found");
        }
        this.MapToModel(manager.Data.DbConnections[repoindex]);
        manager.Save();
    }
}