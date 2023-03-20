namespace SE1607_Group4_A3
{
    public class Constant
    {
        public const string userSessionKey = "user";
        public const string userSessionRoleKey = "userRole";
        public static Constant Instance
        {
            get
            {
                return instance;
            }
        }
        public readonly IConfigurationRoot configuration;
        public Constant()
        {
            configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", false, true).Build();
        }
        private static readonly Constant instance = new();
    }
}
