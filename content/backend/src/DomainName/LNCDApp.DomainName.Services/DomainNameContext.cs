using System;
using System.Security.Claims;
using System.Threading;
using LeanCode.CQRS.Security;
using LeanCode.Pipelines;
using LNCDApp.DomainName.Contracts;
using Microsoft.AspNetCore.Http;

namespace LNCDApp.DomainName.Services
{
    public class DomainNameContext : ISecurityContext
    {
        IPipelineScope IPipelineContext.Scope { get; set; } = null!;

        public ClaimsPrincipal User { get; }
        public CancellationToken CancellationToken { get; }

        private DomainNameContext(ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            User = user;
            CancellationToken = cancellationToken;
        }

        public static DomainNameContext FromHttp(HttpContext httpContext)
        {
            return new DomainNameContext(httpContext.User, httpContext.RequestAborted);
        }

        private static DomainNameContext ForTests(Guid userId, string role)
        {
            var claims = new[]
            {
                new Claim(Auth.KnownClaims.UserId, userId.ToString()),
                new Claim(Auth.KnownClaims.Role, role),
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(
                claims: claims,
                authenticationType: "internal",
                nameType: Auth.KnownClaims.UserId,
                roleType: Auth.KnownClaims.Role));

            return new DomainNameContext(user, default);
        }

        private static Guid ParseUserClaim(ClaimsPrincipal? user, string claimType)
        {
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                var str = user.FindFirstValue(claimType);
                _ = Guid.TryParse(str, out var res);
                return res;
            }
            else
            {
                return Guid.Empty;
            }
        }
    }
}
