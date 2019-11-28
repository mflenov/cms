using System;
using System.Collections.Generic;
using System.Text;

namespace FCms.Content
{
    public class ValueListFilter : IFilter
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Type { get { return "ValueList"; } }

        public bool Validate(List<object> values, object value)
        {
            return false;
        }

    }
}
