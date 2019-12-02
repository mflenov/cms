﻿using System;
using System.Collections.Generic;

namespace FCms.Content
{
    public class DateRangeFilter : IFilter
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Type { get { return "DateRange"; } }

        public bool Validate(List<object> values, object value)
        {
            return false;
        }
    }
}
