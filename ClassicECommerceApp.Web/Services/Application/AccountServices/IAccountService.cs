using ClassicECommerceApp.Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ClassicECommerceApp.Web.Services.Application.AccountServices
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUserAsync(string username, string email, string password);
        Task<SignInResult> LoginUserAsync(string email, string password, bool rememberMe);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string code);
        Task<bool> SendPasswordResetEmailAsync(string email);
        Task<bool> ResetUserPasswordAsync(string email, string token, string newPassword);
        AuthenticationProperties ConfigureExternalAuthenticationProperties(string? provider, string? returnUrl);
        Task<ExternalLoginInfo?> GetExternalLoginInfoAsync();
        Task<SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo info);
        Task<bool> RegisterUserIfNotExistAsync(ExternalLoginInfo info);
        Task<ApplicationUser?> FindByLoginAsync(string loginProvider, string providerKey);
        Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims);
        Task SignInAsync(ApplicationUser user);
    }
}
