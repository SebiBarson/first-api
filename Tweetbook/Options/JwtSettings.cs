using System;

namespace Tweetbook.Options
{
    public class JwtSettings
    {
        public TimeSpan TokenLifetime { get; set; }
        public string Secret { get; set; }
    }
}
