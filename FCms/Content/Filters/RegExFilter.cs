using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FCms.Content
{
    public class RegExFilter : IFilter
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Type { get { return "RegEx"; } }

        public bool Validate(List<object> values, object value)
        {
            if (values == null)
            {
                return false;
            }

            foreach (string pattern in values)
            {
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                if (regex.IsMatch((string)value))
                {
                    return true;
                }
            }
            return false;
        }

        public List<object> ParseValues(List<string> list)
        {
            return list.Select(m => (object)m).ToList();
        }
    }
}
