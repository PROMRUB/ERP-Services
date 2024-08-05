﻿using ERP.Services.API.Models.Authentications;
using ERP.Services.API.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Serilog;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ERP.Services.API.Authentications
{
    public abstract class AuthenticationHandlerProxyBase : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        protected abstract Task<Models.Authentications.User>? AuthenticateBasic(string orgId, byte[]? jwtBytes, HttpRequest request);
        protected abstract Task<Models.Authentications.User>? AuthenticateBearer(string orgId, byte[]? jwtBytes, HttpRequest request);

        protected AuthenticationHandlerProxyBase(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authData))
            {
                return AuthenticateResult.Fail("No Authorization header found");
            }

            var authHeader = AuthenticationHeaderValue.Parse(authData);
            if (!authHeader.Scheme.Equals("Bearer") && !authHeader.Scheme.Equals("Basic"))
            {
                return AuthenticateResult.Fail($"Unknown scheme [{authHeader.Scheme}]");
            }

            Models.Authentications.User? user = null;
            try
            {
                var orgId = ServiceUtils.GetOrgId(Request);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);

                if (authHeader.Scheme.Equals("Basic"))
                {
                    user = await Task.Run(() => AuthenticateBasic(orgId, credentialBytes, Request));
                }
                else
                {
                    user = await Task.Run(() => AuthenticateBearer(orgId, credentialBytes, Request));
                }
            }
            catch (Exception e)
            {
                Log.Error($"[AuthenticationHandlerProxyBase] --> [{e.Message}]");
                return AuthenticateResult.Fail($"Invalid Authorization Header for [{authHeader.Scheme}]");
            }

            if (user == null)
            {
                return AuthenticateResult.Fail($"Invalid username or password for [{authHeader.Scheme}]");
            }

            var identity = new ClaimsIdentity(user.Claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            Context.Request.Headers.Add("AuthenScheme", Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
