using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Data;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;
        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<bool> CreatePostAsync(Post post)
        {
            await _dataContext.Posts.AddAsync(post);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }
        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);
            if (post == null)
            {
                return false;
            }
            _dataContext.Posts.Remove(post);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }
        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dataContext.Posts.Include(p => p.Tags).ThenInclude(pt => pt.Tag).SingleOrDefaultAsync(x => x.Id == postId);
        }
        public async Task<List<Post>> GetPostsAsync()
        {
            List<Post> posts = await _dataContext.Posts.Include(p => p.Tags).ThenInclude(pt => pt.Tag).ToListAsync();
            return posts;
        }
        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            var post = await GetPostByIdAsync(postToUpdate.Id);
            if (post == null)
            {
                return false;
            }
            post.Name = postToUpdate.Name;
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;

        }
        public async Task<bool> UserOwnsPostAsync(Guid postId, string getUserId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);
            if (post == null)
            {
                return false;
            }
            if (post.UserId != getUserId)
            {
                return false;
            }
            return true;
        }
    }
}