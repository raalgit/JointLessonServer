using System.Web.Http.Filters;

namespace JLServer.Attributes
{
    public class JwtAuthenticationAttribute : AuthorizeAttribute, IAuthenticationFilter
    {
        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public JwtAuthenticationAttribute(string role) : base(role)
        {

        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
        }


        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}
