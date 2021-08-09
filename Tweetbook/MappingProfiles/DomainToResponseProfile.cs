using AutoMapper;
using System.Linq;
using Tweetbook.Contracts.V1.Responses;
using Tweetbook.Domain;

namespace Tweetbook.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Post, PostResponse>()
                .ForMember(dest => dest.Tags, opt =>
                opt.MapFrom(src => src.Tags.Select(x =>
                new TagResponse
                {
                    Id = x.Tag.Id,
                    Name = x.Tag.Name,
                    CreatedOn = x.Tag.CreatedOn,
                    CreatorId = x.Tag.CreatorId
                }).ToList()));
            CreateMap<Tag, TagResponse>();
        }
    }
}
