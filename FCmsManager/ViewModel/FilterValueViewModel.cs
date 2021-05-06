using FCms.Content;

namespace FCmsManager.ViewModel
{
    public class FilterValueViewModel
    {
        public IFilter FilterDefinition { get; set; }

        public int Index { get; set; }

        public IContentFilter ContentFilter { get; set; }
    }
}
