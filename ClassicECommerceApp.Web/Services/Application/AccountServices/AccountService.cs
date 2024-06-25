using ClassicECommerceApp.Data.Entities;
using ClassicECommerceApp.Web.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Security.Claims;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace ClassicECommerceApp.Web.Services.Application.AccountServices
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountService> _logger;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUrlHelperFactory urlHelperFactory, IHttpContextAccessor httpContextAccessor,
            IEmailSender emailSender, ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _urlHelperFactory = urlHelperFactory;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
            _logger = logger;
        }
        public async Task<IdentityResult> RegisterUserAsync(string username, string email, string password)
        {
            // Check if email already exists
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "DuplicateEmail",
                    Description = "Email is already taken."
                });
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = username,
                Email = email,
            };

            IdentityResult result;

            try
            {
                result = await _userManager.CreateAsync(user, password);
            }
            catch (Exception ex)
            {
                // Handle possible exceptions like duplicate email due to race conditions
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "Exception",
                    Description = $"An error occurred: {ex.Message}"
                });
            }

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // Check if HttpContext is null
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    throw new InvalidOperationException("Cannot generate URL: HttpContext is not available.");
                }

                var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext
                {
                    HttpContext = httpContext,
                    RouteData = httpContext.GetRouteData(),
                    ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
                });

                var callbackUrl = urlHelper.Action(
                    "ConfirmEmail",
                    "UserAccount",
                    new { userId = user.Id, code = code },
                    protocol: httpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(email, "Confirm your email",
                    $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.");
            }

            return result;
        }

        public async Task<SignInResult> LoginUserAsync(string email, string password, bool rememberMe)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new EmailConfirmationException("User not found");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return result;
        }

        public async Task<bool> SendPasswordResetEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Do not reveal that the user does not exist or is not confirmed
                return false;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Check if HttpContext is null
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new InvalidOperationException("Cannot generate URL: HttpContext is not available.");
            }

            var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext
            {
                HttpContext = httpContext,
                RouteData = httpContext.GetRouteData(),
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor()
            });

            var callbackUrl = urlHelper.Action("ResetPassword", "UserAccount", new { token, email }, protocol: httpContext.Request.Scheme);

            await _emailSender.SendEmailAsync(email, "Reset Password",
                $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

            return true;
        }

        public async Task<bool> ResetUserPasswordAsync(string email, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }

        public AuthenticationProperties ConfigureExternalAuthenticationProperties(string? provider, string? returnUrl)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, returnUrl);
            return properties;
        }

        public async Task<ExternalLoginInfo?> GetExternalLoginInfoAsync()
        {
            return await _signInManager.GetExternalLoginInfoAsync();
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(ExternalLoginInfo info)
        {
            return await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: false);
        }

        public async Task<bool> RegisterUserIfNotExistAsync(ExternalLoginInfo info)
        {
            var email = info.Principal.FindFirstValue(ClaimTypes.Email) ?? "";
            var user = await _userManager.FindByEmailAsync(email!);
            if (user == null)
            {
                // Create new user if it doesn't exist
                var newUser = new ApplicationUser { UserName = email, Email = email };
                var createResult = await _userManager.CreateAsync(newUser);

                if (createResult.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var emailConfirmed = await _userManager.ConfirmEmailAsync(newUser, token);
                    var addLoginResult = await _userManager.AddLoginAsync(newUser, info);

                    if (addLoginResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(newUser, isPersistent: false);
                        return true; // indicate that login success -> redirect in controller
                    }
                    else
                    {
                        _logger.LogError($"Failed to add external login for user {email}");
                        return false;
                    }
                }
                else
                {
                    _logger.LogError($"Failed to create user {email}");
                    return false;
                }
            }
            else
            {
                _logger.LogWarning($"User {email} exists but failed to sign in due to some reason.");
                return false;
            }

        }

        public async Task<ApplicationUser?> FindByLoginAsync(string loginProvider, string providerKey)
        {
            // Find user by external login provider and provider key
            return await _userManager.FindByLoginAsync(loginProvider, providerKey);
        }

        public async Task AddClaimsAsync(ApplicationUser user, IEnumerable<Claim> claims)
        {
            // Add claims to the user
            await _userManager.AddClaimsAsync(user, claims);
        }
        public async Task SignInAsync(ApplicationUser user)
        {
            // Sign in the user again to refresh the claims in the current session
            await _signInManager.RefreshSignInAsync(user);
        }
    }
}
