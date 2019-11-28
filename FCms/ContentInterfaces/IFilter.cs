using System;
using System.Collections.Generic;
using System.Text;

namespace FCms.Content
{
    public interface IFilter
    {
        public enum FilterType  { Text, Boolean, RegEx, DateRange, ValueList }

        Guid Id { get; set; }

        string Name { get; set; }

        string Type { get; }

        bool Validate(List<object> values, object value);
    }
}
