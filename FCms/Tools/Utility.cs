using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace FCms.Tools
{
    public static class Utility
    {
        public static int? StringToInt(string value)
        {
            if (int.TryParse(value, out var intValue)) {
                return intValue;
            }
            return null;
        }

        public static int StringToIntDef(string value, int defValue)
        {
            return int.TryParse(value, out var intValue) ? intValue : defValue;
        }

        public static string GetRequestValueDef(HttpRequest request, string name, string defaultvalue)
        {
            if (request?.Form == null)
            {
                return defaultvalue;
            }
            return request.Form.ContainsKey(name) ? request.Form[name][0] : defaultvalue;
        }

        public static List<string> GetRequestList(HttpRequest request, string name)
        {
            if (request?.Form == null)
            {
                return new List<string>();
            }
            return request.Form.ContainsKey(name) ? request.Form[name].Select(m => m).ToList() : new List<string>();
        }
        public static int GetRequestIntValueDef(HttpRequest request, string name, int defaultvalue)
        {
            return StringToIntDef(Utility.GetRequestValueDef(request, name, ""), defaultvalue);
        }
    }
}
