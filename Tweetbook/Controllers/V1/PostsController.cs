using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetbook.Cache;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Extensions;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
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
        /// <summary>
        /// Returns all the posts in the system
        /// </summary>  
        /// <response code="201">Returns all the posts in the system</response>
        [ProducesResponseType(typeof(List<PostResponse>), 201)]
        [Cached(600)]
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _postService.GetPostsAsync());
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
            {
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Not found" } } });
            }
            return Ok(_mapper.Map<PostResponse>(post));
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new ErrorResponse
                {
                    Errors = new List<ErrorModel> { new ErrorModel
                    { Message = "You do not own this post or this post does not exist" } }
                });

            var post = await _postService.GetPostByIdAsync(postId);
            post.Name = request.Name;
            var updated = await _postService.UpdatePostAsync(post);
            if (updated)
            {
                return Ok(_mapper.Map<PostResponse>(post));
            }
            return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Not found" } } });
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPostAsync(postId, HttpContext.GetUserId());
            if (!userOwnsPost)
            {
                return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "You do not own this post or this post does not exist" } } });
            }
            var deleted = await _postService.DeletePostAsync(postId);
            if (deleted)
                return NoContent();
            return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Not found" } } });
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest request)
        {
            var tags = new List<Tag>();
            foreach (string tagName in request.Tags)
            {
                Tag tag = new() { Name = tagName, CreatedOn = DateTime.Now, CreatorId = HttpContext.GetUserId() };
                tags.Add(tag);
                await _tagService.CreateTagAsync(tag);
            }

            var post = new Post { Name = request.Name, UserId = HttpContext.GetUserId() };

            var created = await _postService.CreatePostAsync(post);
            if (!created)
            {
                return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to create a post" } } });
            }

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
