using System.Collections.Generic;

namespace FCms
{
    public static class HttpContext
    {
        private static Microsoft.AspNetCore.Http.IHttpContextAccessor m_httpContextAccessor;


        public static void Configure(Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor)
        {
            m_httpContextAccessor = httpContextAccessor;
        }


        public static IDictionary<object, object> RequestCache
        {
            get
            {
                if (m_httpContextAccessor == null)
                {
                    return new Dictionary<object, object>();
                }
                return m_httpContextAccessor.HttpContext.Items;
            }
        }


    }
}
