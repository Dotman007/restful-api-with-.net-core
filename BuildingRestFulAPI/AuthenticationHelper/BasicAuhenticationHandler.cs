using BuildingRestFulAPI.DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.AuthenticationHelper
{
    public class BasicAuhenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly StoreContext _context;
        public BasicAuhenticationHandler(IOptionsMonitor<BasicAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, StoreContext context)
            : base(options, logger, encoder, clock)
        {
            _context = context;
        }
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = $"Basic realm=\"http://localhost:51985\",charset=\"UTF-8\"";
            await base.HandleChallengeAsync(properties);
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //Verify if the AuthorizationHeaderName is has the value Authorization.
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            //Verify if the header value is valid.
            if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out AuthenticationHeaderValue headerValue))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            //Verify if the scheme name has a value Basic.
            if (!headerValue.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            byte[] headerValueByte = Convert.FromBase64String(headerValue.Parameter);
            string emailPassword = Encoding.UTF8.GetString(headerValueByte);
            string[] parts = emailPassword.Split(":");
            //Verifies if the Length of  the part is 2.
            if (parts.Length != 2)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Basic Authentication Scheme"));
            }
            var email = parts[0];
            var password = parts[1];
            //Checking the email and password in the database.
            var customer = _context.Customers.SingleOrDefault(c => c.Email == email && c.Password == password && c.IsCustomer);
            
            var agent = _context.Customers.SingleOrDefault(c => c.AgentUserName == email && c.Password == password && c.IsAgent == true);
           
            //Check if the email and password is null.
            IList<Claim> claims = new List<Claim>();
            if (customer != null)
            {
                if (customer != null)
                {
                    claims = new[]
                {
                new Claim(ClaimTypes.Name,email),
                new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
            };
                }
            }
            

            if (agent != null)
            {
                claims = new[]
            {
                new Claim(ClaimTypes.Name,email),
                new Claim(ClaimTypes.NameIdentifier, agent.Id.ToString()),
            };
            }
            //Create claim with email and id.

            //ClaimsIdentity Creation with claim.
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
