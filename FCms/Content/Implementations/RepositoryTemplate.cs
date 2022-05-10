using System;
namespace FCms.Content
{
    public static class RepositoryTemplate
    {

        public static void ApplyTemplate(ContentTemplate template, IRepository model)
        {
            if (model == null)
            {
                return;
            }
            if (template == ContentTemplate.SimplePage)
            {
                model.AddDefinition("Title", ContentDefinitionType.String);
                model.AddDefinition("Description", ContentDefinitionType.String);
                model.AddDefinition("Content", ContentDefinitionType.LongString);
            }
        }
    }
}
