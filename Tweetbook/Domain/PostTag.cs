using System;
using System.ComponentModel.DataAnnotations;

namespace Tweetbook.Domain
{
    public class PostTag
    {
        [Key]
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Post Post { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}