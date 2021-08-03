using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public interface ITagService
    {
        public Task<bool> CreateTagAsync(Tag tag);
        public Task<List<Tag>> GetAllTagsAsync();
        public Task<List<Tag>> GetTagsForPostAsync(Guid postId);
        public Task<bool> CreatePost_TagAsync(PostTag post_Tag);
    }
}
