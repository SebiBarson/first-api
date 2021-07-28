using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Requests;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Services;
using Tweetbook.Extensions;

namespace Tweetbook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : Controller
    {
        private IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

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
                return NotFound();

            return Ok(post);
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var userOwnsPost = await _postService.UserOwnsPost(postId, HttpContext.GetUserId());

            if (!userOwnsPost)
                return BadRequest(new { error = "You don't own this post" });

            var post = new Post
            {
                Id = postId,
                Name = request.Name
            };

            var updated = await _postService.UpdatePostAsync(post);
            return Ok(post);
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var userOwnsPost = await _postService.UserOwnsPost(postId, HttpContext.GetUserId());

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
            var post = new Post
            {
                UserId = HttpContext.GetUserId(),
                Name = postRequest.Name
            };

            await _postService.CreatePostAsync(post); 
            var response = new PostResponse { Id = post.Id };
            return Created(UriLocation(post), response);
        }

        private string UriLocation(Post post)
        {
            var baseUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.ToUriComponent();
            var uriLocation = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());
            return uriLocation;
        }
    }
}
