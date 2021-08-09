﻿using System;

namespace Tweetbook.Contracts.V1.Responses
{
    public class TagResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CreatorId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}