using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Data;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public class TagService : ITagService
    {
        private readonly DataContext _dataContext;

        public TagService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Tag>> GetTagsForPostAsync(Guid postId)
        {
            //take from many to many table
            List<Guid> tagIds = await _dataContext.Posts_Tags.Where(pt => pt.PostId == postId).Select(pt => pt.PostId).ToListAsync();
            List<Tag> tags = new();
            foreach (Guid tagId in tagIds)
            {
                tags.Add(await _dataContext.Tags.Where(t => t.Id == tagId).SingleOrDefaultAsync());
            }
            return tags;
        }


        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _dataContext.Tags.ToListAsync();
        }


        public async Task<bool> CreateTagAsync(Tag tag)
        {
            await _dataContext.Tags.AddAsync(tag);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> CreatePost_TagAsync(PostTag post_Tag)
        {
            await _dataContext.Posts_Tags.AddAsync(post_Tag);
            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }
    }
}
