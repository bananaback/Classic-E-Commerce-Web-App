using ClassicECommerceApp.Data.Entities;
using ClassicECommerceApp.Web.Exceptions;
using ClassicECommerceApp.Web.Models.Results;
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

        public async Task<ExternalLoginResult> ExternalLoginAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _logger.LogError("No external login info found");
                return ExternalLoginResult.NoInfoFailure();
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email) ?? "";
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var createUserResult = await CreateNewUserAsync(email, info);
                if (!createUserResult.IsSuccess)
                {
                    return createUserResult;
                }
                user = createUserResult.User;
            }
            else
            {
                var userWithExternalProvider = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (userWithExternalProvider == null)
                {
                    var addLoginResult = await AddExternalLoginToUserAsync(user, info);
                    if (!addLoginResult.IsSuccess)
                    {
                        return addLoginResult;
                    }
                }
            }

            // Add picture claim if available
            await AddPictureClaimAsync(user!, info);

            var loginResult = await SignInUserWithExternalLoginAsync(info);
            return loginResult;
        }

        private async Task<ExternalLoginResult> CreateNewUserAsync(string email, ExternalLoginInfo info)
        {
            var newUser = new ApplicationUser { UserName = email, Email = email };
            var createUserResult = await _userManager.CreateAsync(newUser);

            if (!createUserResult.Succeeded)
            {
                _logger.LogError($"Failed to create user {email}");
                return ExternalLoginResult.CreateNewUserFailure();
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            await _userManager.ConfirmEmailAsync(newUser, token);

            var addLoginResult = await AddExternalLoginToUserAsync(newUser, info);
            return addLoginResult.IsSuccess ? ExternalLoginResult.Success(newUser) : addLoginResult;
        }

        private async Task<ExternalLoginResult> AddExternalLoginToUserAsync(ApplicationUser user, ExternalLoginInfo info)
        {
            var addExternalLoginProviderResult = await _userManager.AddLoginAsync(user, info);
            if (!addExternalLoginProviderResult.Succeeded)
            {
                _logger.LogError($"Failed to add external login for user {user.Email}");
                return ExternalLoginResult.CreateExternalProviderFailure();
            }
            return ExternalLoginResult.Success(user);
        }

        private async Task<ExternalLoginResult> SignInUserWithExternalLoginAsync(ExternalLoginInfo info)
        {
            var loginResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: false);
            return loginResult.Succeeded ? ExternalLoginResult.Success() : ExternalLoginResult.ExternalLoginFailure();
        }

        private async Task AddPictureClaimAsync(ApplicationUser user, ExternalLoginInfo info)
        {
            var pictureClaim = info.Principal.FindFirstValue("picture") ?? "/images/default-profile-picture.png"; // Use default picture if none found

            var currentClaims = await _userManager.GetClaimsAsync(user);
            var existingPictureClaim = currentClaims.FirstOrDefault(c => c.Type == "picture");

            if (existingPictureClaim != null)
            {
                var replaceClaimResult = await _userManager.ReplaceClaimAsync(user, existingPictureClaim, new Claim("picture", pictureClaim));
                if (!replaceClaimResult.Succeeded)
                {
                    _logger.LogError($"Failed to replace picture claim for user {user.Email}");
                }
            }
            else
            {
                var addClaimResult = await _userManager.AddClaimAsync(user, new Claim("picture", pictureClaim));
                if (!addClaimResult.Succeeded)
                {
                    _logger.LogError($"Failed to add picture claim for user {user.Email}");
                }
            }
        }

        public async Task LogUserOut()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
