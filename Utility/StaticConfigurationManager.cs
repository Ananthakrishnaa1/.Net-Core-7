namespace Main.Service.Utility
{
    static class StaticConfigurationManager
    {
        public static IConfiguration AppSetting
        {
            get;
        }
        static StaticConfigurationManager()
        {
            AppSetting = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build();
        }
    }
}
