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
                model.AddDefinition("Title", IContentDefinition.DefinitionType.String);
                model.AddDefinition("Description", IContentDefinition.DefinitionType.String);
                model.AddDefinition("Content", IContentDefinition.DefinitionType.LongString);
            }
        }
    }
}
