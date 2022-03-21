using System;
using System.Linq;
using System.Collections.Generic;

namespace FCms.Content
{
    public class TextFilter : IFilter
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Type { get { return "Text"; } }

        public bool Validate(List<object> values, object value)
        {
            if (values == null)
            {
                return false;
            }
            return values.Contains(value);
        }
        public List<object> ParseValues(List<string> list)
        {
            return list.Select(m => (object)m).ToList();
        }
    }
}
