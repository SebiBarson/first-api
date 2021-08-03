using System.Collections.Generic;

namespace Tweetbook.Contracts.V1.Requests
{
    public class CreatePostRequest
    {
        //public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
