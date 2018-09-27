// <copyright file="Constants.cs">
//    2017 - Johan Boström
// </copyright>

namespace WasteProducts.IdentityServer
{
    public static class Constants
    {
        public const string secretApi_webApi = "e155d0c6-0fd9-437b-ae6c-d1534ed115ca";
        public const string secretApi_angular = "a3c71678-24b7-4c16-bace-f3ef14e1a228";

        public const string secret_write_ = @"4cba5bd3-0c32-49fc-8fe9-b640b10edff6";
        public const string secret_read_ = @"7860353a-7209-4a65-ada3-e1229a5f7352";
        public const string secret_api_ = @"51cb25ed-7753-45ff-8b93-2cb9e1b0dc43";

        public const string ConnectionStringName = "DefaultConnection";

        public class Routes
        {
            public const string Core = "/core";
            public const string IdMgr = "/idm";
            public const string IdAdm = "/ida";
        }
    }
}