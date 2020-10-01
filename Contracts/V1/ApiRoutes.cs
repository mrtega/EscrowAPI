using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscrowAPI.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;
        public static class Transaction
        {
            public const string GetAll = Base + "/transaction";

            public const string Get = Base + "/transaction/{transactionId}";

            public const string Update = Base + "/transaction/{transactionId}";

            public const string Delete = Base + "/transaction/{transactionId}";

            public const string Create = Base + "/transaction";
            //public const string Get = "api/v1/transaction{postId}";
        }

        public static class Identity
        {
            public const string Login = Base + "/identity/login";

            public const string Register = Base + "/identity/Register";

            public const string Refresh = Base + "/identity/refresh";
        }
    }
}
