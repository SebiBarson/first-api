using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetbook.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Route = "api";
        public const string Version = "v1";
        public const string Base = Route + "/" + Version;
        public static class Posts
        {
            public const string GetAll = Base + "/posts";
        }
    }
}
