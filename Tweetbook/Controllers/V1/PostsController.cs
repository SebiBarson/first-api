using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Extensions;
using Tweetbook.Services;
using AutoMapper;

namespace Tweetbook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;

        public PostsController(IPostService postService, ITagService tagService, IMapper mapper)
        {
            _postService = postService;
            _tagService = tagService;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _postService.GetPostsAsync();
            var postResponses = _mapper.Map<List<PostResponse>>(posts);
            return Ok(postResponses);
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);

            if (post == null)
                return NotFound();

            return Ok(_mapper.Map<PostResponse>(post));
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new { error = "You don't own this post" });

            var post = new Post
            {
                Id = postId,
                Name = request.Name
            };
            var updated = await _postService.UpdatePostAsync(post);
            return Ok(_mapper.Map<PostResponse>(post));
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new { error = "You don't own this post" });

            var deleted = await _postService.DeletePostAsync(postId);

            if (deleted)
                return NoContent();

            return NotFound();
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var tags = new List<Tag>();
            foreach (string tagName in postRequest.Tags)
            {
                Tag tag = new() { Name = tagName, CreatedOn = DateTime.Now, CreatorId = HttpContext.GetUserId() };
                tags.Add(tag);
                await _tagService.CreateTagAsync(tag);
            }

            var post = new Post
            {
                UserId = HttpContext.GetUserId(),
                Name = postRequest.Name
            };

            await _postService.CreatePostAsync(post);

            foreach (Tag tag in tags)
            {
                await _tagService.CreatePostTagAsync(new PostTag { PostId = post.Id, TagId = tag.Id });
            }

            return Created(UriLocation(post), _mapper.Map<PostResponse>(post));
        }

        private string UriLocation(Post post)
        {
            var baseUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.ToUriComponent();
            var uriLocation = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());
            return uriLocation;
        }
    }
}
