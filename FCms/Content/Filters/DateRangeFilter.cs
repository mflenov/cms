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
        public List<object> ParseValues(List<string> list)
        {
            List<object> result = new List<object>();
            foreach (string item in list ?? new List<string>())
            {
                DateTime? value = FCms.Tools.Utility.StringToDateTime(item);
                if (value != null)
                {
                    result.Add(value);
                }
            }
            return result;
        }
    }
}
