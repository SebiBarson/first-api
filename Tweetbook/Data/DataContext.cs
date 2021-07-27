using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using Tweetbook.Domain;

namespace Tweetbook.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public Microsoft.EntityFrameworkCore.DbSet<Post> Posts { get; set; }
    }
}