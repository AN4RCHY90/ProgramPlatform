﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProgramPlatform.Models;

namespace ProgramPlatform.Areas.Identity
{
    public class CustomSignInManager(
        UserManager<ApplicationUserModel> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<ApplicationUserModel> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<ApplicationUserModel>> logger,
        IAuthenticationSchemeProvider schemes,
        IUserConfirmation<ApplicationUserModel> confirmation)
        : SignInManager<ApplicationUserModel>(userManager, contextAccessor, claimsFactory,
            optionsAccessor, logger, schemes, confirmation)
    {
        /// <summary>
        /// Asynchronously signs in a user with the specified email and password.
        /// </summary>
        /// <param name="email">The email address of the user to sign in.</param>
        /// <param name="password">The password of the user to sign in.</param>
        /// <param name="isPersistent">True to create a persistent cookie, false otherwise.</param>
        /// <param name="lockoutOnFailure">True to lockout the user if sign in fails, false otherwise.</param>
        /// <returns>A Task that represents the asynchronous sign-in operation. The task result
        /// contains the result of the sign-in attempt (success or failure).</returns>
        public override async Task<SignInResult> PasswordSignInAsync(string email, string password,
            bool isPersistent, bool lockoutOnFailure)
        {
            return await base.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure);
        }
    }
}