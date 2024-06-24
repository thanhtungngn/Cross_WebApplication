namespace Cross_WebApplication.Models
{
    public static class AppConstant
    {
        public static class Role
        {
            public const string Reader = "Reader";
            public const string Contributor = "Contributor";
            public const string Admin = "Admin";
        }

        public static List<string> RoleList = new List<string>
        {
            Role.Reader, Role.Contributor, Role.Admin
        };
    }
}
