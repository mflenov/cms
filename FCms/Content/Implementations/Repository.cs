using FCms.Content;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FCms.Content
{
    public class Repository: IRepository
    {
        #region definition

        public Guid Id { get; set; }

        public string Name { get; set; }

        public ContentType ContentType { get; set; }

        List<IContentDefinition> contentDefinitions = new List<IContentDefinition>();
        public List<IContentDefinition> ContentDefinitions {
            get { return contentDefinitions; }
            set { contentDefinitions = value.ToList(); }
        }


        public Repository()
        {
        }

        #endregion

        #region store 

        public void mapNewProperties(IRepository repo)
        {
            if (repo == null)
            {
                throw new NullReferenceException();
            }
            this.Name = repo.Name;
        }

        public IContentDefinition GetByName(string contentName)
        {
            return ContentDefinitions.Where(m => m.Name == contentName).FirstOrDefault();
        }

        public void DeleteDefinition(Guid id)
        {
            var item = ContentDefinitions.Where(m => m.DefinitionId == id).FirstOrDefault();
            if (item != null)
            {
                contentDefinitions.Remove(item);
            }

        }

        public void AddDefinition(string name, IContentDefinition.DefinitionType type)
        {
            if (contentDefinitions.Any(m => m.Name == name))
            {
                return;
            }
            var definition = ContentDefinitionFactory.CreateContentDefinition(type);
            definition.DefinitionId = Guid.NewGuid();
            definition.Name = name;
            contentDefinitions.Add(definition);
        }

        #endregion
    }
}
