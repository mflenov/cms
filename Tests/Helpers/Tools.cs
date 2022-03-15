using System;
using System.Collections.Generic;
using System.Text;

namespace FCmsTests.Helpers
{
    static class Tools
    {
        public static void DeleteCmsFile()
        {
            if (System.IO.File.Exists(Constants.CmsFilename))
            {
                System.IO.File.Delete(Constants.CmsFilename);
            }
        }
    }
}
