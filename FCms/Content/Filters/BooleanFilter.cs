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
            return values.Contains((bool)value);
        }

        public List<object> ParseValues(List<string> list)
        {
            List<object> result = new List<object>();
            foreach (string item in list ?? new List<string>())
            {
                bool? value = FCms.Tools.Utility.StringToBoolean(item);
                result.Add(value == true);
            }
            if (list.Count == 0)
            {
                result.Add(false);
            }
            return result;
        }
    }
}
