using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tweetbook.Cache;
using Tweetbook.Services;

namespace Tweetbook.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
            var redisCacheSettings = new RedisCacheSettings();
            configuration.GetSection(nameof(redisCacheSettings)).Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);
            if (!redisCacheSettings.Enabled)
            {
                return;
            }
            services.AddStackExchangeRedisCache(options => { options.Configuration = redisCacheSettings.ConnectionString; });
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}
