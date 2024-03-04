using CarShare.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace CarShare.Areas.Extends
{
    public static class SignInMenagerExtend
    {
        public static async Task<SignInResult> PasswordSignInByEmailAsync(this SignInManager<CarShareUser> signInManager , string userName, string password,
            bool isPersistent, bool lockoutOnFailure)
        {
            var user = await signInManager.UserManager.FindByEmailAsync(userName);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }
    }
}
