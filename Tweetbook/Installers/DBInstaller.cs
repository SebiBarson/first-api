using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tweetbook.Data;
using Tweetbook.Services;

namespace Tweetbook.Installers
{
    public class DBInstaller : IInstaller
    {
        public void InstallServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentityCore<IdentityUser>().AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ITagService, TagService>();
        }
    }
}
