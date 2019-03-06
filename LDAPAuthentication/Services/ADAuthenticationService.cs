using LDAPAuthentication.Constants;
using LDAPAuthentication.Models;
using Microsoft.Owin.Security;
using System;
using System.DirectoryServices.AccountManagement;
using System.Security.Claims;

namespace LDAPAuthentication.Services
{
    public class ADAuthenticationService
    {
        private readonly IAuthenticationManager authenticationManager;

        public ADAuthenticationService(IAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }

        public AuthenticationResult SignIn(string username, string password)
        {
            PrincipalContext principalContext = new PrincipalContext(ContextType.Domain);
            bool isAuthenticated = false;
            UserPrincipal userPrincipal = null;

            try
            {
                isAuthenticated = principalContext.ValidateCredentials(username, password, ContextOptions.Negotiate);

                if (isAuthenticated)
                {
                    userPrincipal = UserPrincipal.FindByIdentity(principalContext, username);
                }
            }
            catch (Exception)
            {
                isAuthenticated = false;
                userPrincipal = null;
            }

            if (!isAuthenticated || userPrincipal == null)
                return AuthenticationResult.FAILED("Username or Password is not correct.");

            if (userPrincipal.IsAccountLockedOut())
                return AuthenticationResult.FAILED("Your account is locked.");

            if (userPrincipal.Enabled.HasValue && userPrincipal.Enabled.Value == false)
                return AuthenticationResult.FAILED("Your account is disabled.");

            ClaimsIdentity identity = CreateIdentity(userPrincipal);

            authenticationManager.SignOut(AUTHENTICATION.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

            return AuthenticationResult.SUCCESS();
        }

        private ClaimsIdentity CreateIdentity(UserPrincipal userPrincipal)
        {
            ClaimsIdentity identity = new ClaimsIdentity(
                AUTHENTICATION.ApplicationCookie, 
                ClaimsIdentity.DefaultNameClaimType, 
                ClaimsIdentity.DefaultRoleClaimType
                );

            identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Active Directory"));
            identity.AddClaim(new Claim(ClaimTypes.Name, userPrincipal.SamAccountName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userPrincipal.SamAccountName));

            if (!string.IsNullOrEmpty(userPrincipal.EmailAddress))
                identity.AddClaim(new Claim(ClaimTypes.Email, userPrincipal.EmailAddress));

            return identity;
        }
    }
}