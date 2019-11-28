using System;
using System.Collections.Generic;
using System.Text;
using FCms.Content;

namespace FCms.Factory
{
    public static class FilterFactory
    {
        public static IFilter CreateFilterByType(IFilter.FilterType filterType)
        {
            switch (filterType)
            {
                case IFilter.FilterType.Boolean:
                    return new BooleanFilter();
                case IFilter.FilterType.DateRange:
                    return new DateRangeFilter();
                case IFilter.FilterType.RegEx:
                    return new RegExFilter();
                case IFilter.FilterType.Text:
                    return new TextFilter();
                case IFilter.FilterType.ValueList:
                    return new ValueListFilter();
                default:
                    throw new NotSupportedException();
            }
        }

        public static IFilter CreateFilterByTypeName(string name)
        {
            IFilter.FilterType filterType = (IFilter.FilterType)Enum.Parse(typeof(IFilter.FilterType), name);
            return CreateFilterByType(filterType);
        }
    }
}
