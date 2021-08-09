using AutoMapper;
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

namespace Tweetbook.Controllers.V1
{
    public class TagsController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;
        public TagsController(ITagService tagService, IMapper mapper)
        {
            _tagService = tagService;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var tags = await _tagService.GetAllTagsAsync();
            return Ok(_mapper.Map<List<TagResponse>>(tags));
        }

        [HttpGet(ApiRoutes.Tags.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid tagId)
        {
            var tag = await _tagService.GetTagByIdAsync(tagId);
            if (tag == null)
                return NotFound(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Not found" } } });
            return Ok(_mapper.Map<TagResponse>(tag));
        }


        [HttpPost(ApiRoutes.Tags.Create)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
            var tag = new Tag { Name = request.Name, CreatorId = HttpContext.GetUserId(), CreatedOn = DateTime.Now };
            var created = await _tagService.CreateTagAsync(tag);
            if (!created)
                return BadRequest(new ErrorResponse { Errors = new List<ErrorModel> { new ErrorModel { Message = "Unable to create a tag" } } });
            return Created(UriLocation(tag), _mapper.Map<TagResponse>(tag));
        }
        private string UriLocation(Tag tag)
        {
            var baseUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.ToUriComponent();
            var uriLocation = baseUrl + "/" + ApiRoutes.Tags.Get.Replace("{tagId}", tag.Id.ToString());
            return uriLocation;
        }
    }
}
