using System.Linq;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using FCms.Content;

namespace FCms.Extensions
{
    public static class CmsContent
    {
        public static IHtmlContent GetCmsString(this IHtmlHelper htmlHelper, string repositoryName, string contentName)
        {
            ContentEngine engine = new ContentEngine(repositoryName);
            return new HtmlString(
                engine.GetContentString(contentName)
            );
        }

        public static IHtmlContent GetContentAsString(this IHtmlHelper htmlHelper, string repositoryName, string contentName, object filters)
        {
            ContentEngine engine = new ContentEngine(repositoryName);
            ContentItem item = engine.GetContents(contentName, filters).FirstOrDefault();
            return new HtmlString(item == null ? "" : item.GetStringValue());
        }
    }
}
