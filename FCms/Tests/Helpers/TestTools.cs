namespace FCmsTests.Helpers;

static class TestTools
{
    public static void DeleteCmsFile()
    {
        if (System.IO.File.Exists(TestConstants.CmsFilename))
        {
            System.IO.File.Delete(TestConstants.CmsFilename);
        }
        FCms.Content.CmsManager.Reset();
    }
}
