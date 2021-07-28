using System.Threading.Tasks;
using Tweetbook.Domain;

namespace Tweetbook.Services
{
    public interface IIdentityService
    {
        public Task<AuthenticationResult> RegisterAsync(string email, string password);
        public Task<AuthenticationResult> LoginAsync(string email, string password);
        public Task<AuthenticationResult> RefreshTokenAsync(string email, string password);
    }
}
