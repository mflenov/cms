using System;
namespace FCmsManagerAngular.ViewModels
{
	public class DbContentItemModel
	{
        public Guid RepositoryId { get; set; }
        public FCms.DbContent.Models.ContentRow Row { get; set; }
    }
}

