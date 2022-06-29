using System;
using System.Collections.Generic;
using System.Text;

namespace FCmsTests.Helpers
{
    static class TestTools
    {
        public static void DeleteCmsFile()
        {
            if (System.IO.File.Exists(TestConstants.CmsFilename))
            {
                System.IO.File.Delete(TestConstants.CmsFilename);
            }
        }
    }
}
