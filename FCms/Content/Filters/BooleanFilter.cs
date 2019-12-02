using System;
using System.Collections.Generic;

namespace FCms.Content
{
    public class BooleanFilter : IFilter
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Type { get { return "Boolean"; } }

        public bool Validate(List<object> values, object value)
        {
            if (values == null)
            {
                return (bool)value == false;
            }
            if ((bool)value == true && values.Contains("on"))
                return true;
            return (bool)value == false && !values.Contains("on");
        }
    }
}
