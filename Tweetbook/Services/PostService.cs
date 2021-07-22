using System;
using System.Collections.Generic;
using System.Linq;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public class _postService : IPostService
    {
        private readonly List<Post> _posts;

        public _postService()
        {
            _posts = new List<Post>();

            for (var i = 0; i < 5; i++)
            {
                _posts.Add(new Post
                {
                    Id = Guid.NewGuid(),
                    Name = "Post name " + i
                });

            }
        }

        public List<Post> GetPosts()
        {
            return _posts;
        }

        public Post GetPostById(Guid postId)
        {
            return _posts.SingleOrDefault(x => x.Id == postId);
        }
    }
}
