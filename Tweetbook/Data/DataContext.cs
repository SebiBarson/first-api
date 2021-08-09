using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Tweetbook.Domain;

namespace Tweetbook.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<PostTag>()
                .HasOne(p => p.Post)
                .WithMany(pt => pt.Tags)
                .HasForeignKey(ti => ti.PostId);
            builder.Entity<PostTag>()
                .HasOne(p => p.Tag)
                .WithMany(pt => pt.PostTags)
                .HasForeignKey(ti => ti.TagId);
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PostTag> Posts_Tags { get; set; }
    }
}