using System;
namespace FCms.Content
{
    public static class RepositoryTemplate
    {

        public static void ApplyTemplate(PageTypeTemplate template, IRepository model)
        {
            if (model == null)
            {
                return;
            }
            if (template == PageTypeTemplate.SimplePage)
            {
                model.AddDefinition("Title", IContentDefinition.DefinitionType.String);
                model.AddDefinition("Description", IContentDefinition.DefinitionType.String);
                model.AddDefinition("Content", IContentDefinition.DefinitionType.LongString);
            }
        }
    }
}
