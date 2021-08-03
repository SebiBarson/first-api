﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tweetbook.Domain
{
    public class Tag
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CreatorId { get; set; }

        [ForeignKey(nameof(CreatorId))]
        public IdentityUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<PostTag> Post_Tags { get; set; }

    }
}