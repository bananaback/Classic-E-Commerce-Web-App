using ClassicECommerceApp.Web.Models.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

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
        Task<ExternalLoginResult> ExternalLoginAsync();
    }
}
