using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using WebAPI.Code;
using WebAPI.Models;

namespace WebAPI.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {

        private readonly string _publicClientId;

        public ApplicationOAuthProvider(string publicClientId)
        {
            if (publicClientId == null)
            {
                throw new ArgumentNullException(nameof(publicClientId));
            }

            _publicClientId = publicClientId;
        }
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);
        }

        public override Task MatchEndpoint(OAuthMatchEndpointContext context)
        {
            if (context.IsTokenEndpoint && context.Request.Method == "OPTIONS")
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "null" });
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Headers", new[] { "authorization" });
                context.RequestCompleted();
                return Task.FromResult(0);
            }

            return base.MatchEndpoint(context);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            try
            {
                var keyInvalidGrant = "invalid_grant";

                var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

                if (allowedOrigin == null) allowedOrigin = "*";

                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

                var valid = false; ;

                try
                {
                    var userInput = new User
                    {
                        Email = Util.Base64Decode(context.UserName),
                        Password = Util.Base64Decode(context.Password)
                    };

                    var userList = Util.LoadJson();

                    valid = userList.Any(usr => usr.Email == userInput.Email && usr.Password == userInput.Password);

                }
                catch (Exception ex)
                {
                    context.SetError(keyInvalidGrant, (ex.Message ?? "no msg"));
                    return;
                }

                var apptItemId = string.Empty;
                if (!valid)
                {
                    context.SetError(keyInvalidGrant, "Login information invalid. Please try again.");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                var dict = new Dictionary<string, string>();
                dict.Add("Result", "Success");

                var props = new AuthenticationProperties(dict);

                var ticket = new AuthenticationTicket(identity, props);
                context.OwinContext.Authentication.SignIn(identity);
                context.Validated(ticket);
            }
            catch (Exception ex) { }
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {

            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            var valid = context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}