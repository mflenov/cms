using System;
using System.Collections.Generic;

namespace FCms.Content;

public interface IFilter
{
    public enum FilterType  { Text, Boolean, RegEx, DateRange, ValueList }

    Guid Id { get; set; }

    string Name { get; set; }

    string DisplayName { get; set; }

    string Type { get; }

    bool Validate(List<object> values, object value);

    List<object> ParseValues(List<string> list);
}
