namespace TWETTY_CHAT
{
    public static class appsettings
    {
        // Somee Internet
        public static string HostUrl { get; } = "http://gavrilovici-chat.somee.com";

        // Home Internet
        //public static string HostUrl { get; } = "http://**.**.**.**";
		
		// Local
        //public static string HostUrl { get; } = "https://localhost:5001";

        public static string ConnectionClientDataBase { get; } = @"Data Source=DB.db;";

    }
}
