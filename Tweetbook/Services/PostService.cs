using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Data;
using Tweetbook.Domain;
using static Tweetbook.Contracts.V1.ApiRoutes;

namespace Tweetbook.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;
        public PostService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<List<Post>> GetPostsAsync()
        {
            return await _dataContext.Posts.ToListAsync();
        }
        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(current_post => current_post.Id == postId);
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            var temp = await GetPostByIdAsync(postToUpdate.Id);
            if(temp != null)
            {
                //temp.Name = postToUpdate.Name;
                _dataContext.Update(postToUpdate);
                var updated = await _dataContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return await CreatePostAsync(postToUpdate);
            }

        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);
            if (post == null)
                return false;

            _dataContext.Posts.Remove(post);
            var deleted = await _dataContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            await _dataContext.AddAsync(post);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UserOwnsPost(Guid postId, string userId)
        {
            var post = await _dataContext.Posts.AsNoTracking().SingleOrDefaultAsync(current_post => current_post.Id == postId);

            if (post == null)
                return false;
            if (post.UserId != userId)
                return false;

            return true;
        }
    }
}
