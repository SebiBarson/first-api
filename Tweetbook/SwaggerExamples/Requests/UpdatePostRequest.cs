using Swashbuckle.AspNetCore.Filters;
using Tweetbook.Contracts.V1.Requests;

namespace Tweetbook.SwaggerExamples.Requests
{
    public class UpdatePostRequestExample : IExamplesProvider<UpdatePostRequest>
    {
        public UpdatePostRequest GetExamples()
        {
            return new UpdatePostRequest
            {
                Name = "new post"
            };
        }
    }
}
