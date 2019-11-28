using System;
using System.Collections.Generic;
using System.Text;

namespace FCms.Content
{
    public class TextFilter : IFilter
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Type { get { return "Text"; } }

        public bool Validate(List<object> values, object value)
        {
            return false;
        }
    }
}
