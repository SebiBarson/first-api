using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tweetbook.Domain
{
    public class PostId
    {
        public Guid Value { get; set; }
        public PostId(Guid val)
        {
            Value = val;
        }

        public static implicit operator PostId(Guid val) => new PostId(val);
        public static implicit operator Guid(PostId val) => val.Value;
    }
}
