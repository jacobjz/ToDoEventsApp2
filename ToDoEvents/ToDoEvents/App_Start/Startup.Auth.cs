﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using Google.Apis.Auth.OAuth2.Responses;
using ToDoEvents.Models;
using Calendar.ASP.NET.MVC5;
using Google.Apis.Util.Store;
using Google.Apis.Auth.OAuth2;

namespace ToDoEvents
{
    public partial class Startup
    {
        private IDataStore dataStore = new FileDataStore(GoogleWebAuthorizationBroker.Folder);
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // ***
            // Enables logging in with the Google login provider.
            var google = new GoogleOAuth2AuthenticationOptions()
            {
                AccessType = "offline",     // Request a refresh token.
                ClientId = MyClientSecrets.ClientId,
                ClientSecret = MyClientSecrets.ClientSecret,
                Provider = new GoogleOAuth2AuthenticationProvider()
                {
                    OnAuthenticated = async context =>
                    {
                        var userId = context.Id;
                        context.Identity.AddClaim(new Claim(MyClaimTypes.GoogleUserId, userId));

                        var tokenResponse = new TokenResponse()
                        {
                            AccessToken = context.AccessToken,
                            RefreshToken = context.RefreshToken,
                            ExpiresInSeconds = (long)context.ExpiresIn.Value.TotalSeconds,
                            Issued = DateTime.Now,
                        };

                        await dataStore.StoreAsync(userId, tokenResponse);
                    },
                },
            };

            foreach (var scope in MyRequestedScopes.Scopes)
            {
                google.Scope.Add(scope);
            }

            app.UseGoogleAuthentication(google);
        }
    }
}