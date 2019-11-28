using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FCms.Tools
{
    public static class Utility
    {
        public static int? StringToInt(string value)
        {
            int intValue;
            if (int.TryParse(value, out intValue)) {
                return intValue;
            }
            return null;
        }

        public static int StringToIntDef(string value, int defValue)
        {
            int intValue;
            if (int.TryParse(value, out intValue))
            {
                return intValue;
            }
            return defValue;
        }

        public static string GetRequestValueDef(HttpRequest request, string name, string defaultvalue)
        {
            if (request == null || request.Form == null)
            {
                return defaultvalue;
            }
            if (request.Form.ContainsKey(name))
            {
                return request.Form[name][0];
            }
            return defaultvalue;
        }

        public static List<string> GetRequestList(HttpRequest request, string name)
        {
            if (request == null || request.Form == null)
            {
                return new List<string>();
            }
            if (request.Form.ContainsKey(name))
            {
                return request.Form[name].Select(m => m).ToList();
            }
            return new List<string>();
        }
        public static int GetRequestIntValueDef(HttpRequest request, string name, int defaultvalue)
        {
            return Utility.StringToIntDef(Utility.GetRequestValueDef(request, name, ""), defaultvalue);
        }
    }
}
