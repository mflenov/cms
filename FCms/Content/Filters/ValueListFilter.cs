using System;
using System.Linq;
using System.Collections.Generic;

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

        public List<object> ParseValues(List<string> list)
        {
            return list.Select(m => (object)m).ToList();
        }
    }
}
