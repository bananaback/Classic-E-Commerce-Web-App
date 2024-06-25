using ClassicECommerceApp.Web.Areas.Account.Models;
using ClassicECommerceApp.Web.Exceptions;
using ClassicECommerceApp.Web.Models.ViewModels;
using ClassicECommerceApp.Web.Services.Application.AccountServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClassicECommerceApp.Web.Areas.Account.Controllers
{
    [Area("Account")]
    public class UserAccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<UserAccountController> _logger;

        public UserAccountController(IAccountService accountService, ILogger<UserAccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmailConfirmed()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("EmailConfirmError");
            }
            try
            {
                var result = await _accountService.ConfirmEmailAsync(userId, code);
                if (result.Succeeded)
                {
                    return View("EmailConfirmed");
                }
                return View("EmailConfirmError");
            }
            catch (EmailConfirmationException ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("EmailConfirmError");
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }


        [HttpGet]
        public IActionResult ForgotPasswordConfirmation(string email)
        {
            ViewData["Email"] = email;
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                var requestId = HttpContext.TraceIdentifier;
                var errorViewModel = new ErrorViewModel
                {
                    RequestId = requestId
                };
                return View("Error", errorViewModel);
            }
            ResetPasswordViewModel resetPasswordViewModel = new ResetPasswordViewModel { Token = token, Email = email };
            return View(resetPasswordViewModel);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation(bool success)
        {
            ViewData["Success"] = success;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.RegisterUserAsync(model.Username, model.Email, model.Password);


            if (result.Succeeded)
            {
                ViewData["Email"] = model.Email;
                return View("RegisterSuccess");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.LoginUserAsync(model.Email, model.Password, false);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "This account has been locked out, please try again later.");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Email not confirmed. Please check your email and confirm your account.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.SendPasswordResetEmailAsync(model.Email);
            if (result)
            {
                return RedirectToAction("ForgotPasswordConfirmation", "UserAccount", new { area = "Account", email = model.Email });
            }
            else
            {
                // Handle the case where sending email fails
                ModelState.AddModelError(string.Empty, "Failed to send reset password email. Please try again later.");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _accountService.ResetUserPasswordAsync(model.Email, model.Token, model.NewPassword);

            return RedirectToAction("ResetPasswordConfirmation", "UserAccount", new { area = "Account", success = result });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider)
        {
            AuthenticationProperties? properties = null;
            // Request a redirect to the external login provider with Challenge()
            // redirectUrl is the url that I should redirect the user after successfully authenticated user in "my app"
            // redirecrUrl in this method != google callback path in program.cs
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback));
            properties = _accountService.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties!, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (remoteError != null)
            {
                _logger.LogError($"Error from external provider: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            // Gets the external login information for the current login, as an asynchronous operation.
            var info = await _accountService.GetExternalLoginInfoAsync();
            if (info == null)
            {
                _logger.LogError($"No external login info found");
                return RedirectToAction(nameof(Login));
            }

            // Extract the picture claim from the external provider
            var pictureClaim = info.Principal.FindFirstValue("picture");
            if (pictureClaim == null)
            {
                _logger.LogError($"No picture claim found from external provider");
                // Handle the case where no picture claim is found, if necessary
            }

            // Sign in user in my app
            var result = await _accountService.ExternalLoginSignInAsync(info);
            if (result.Succeeded)
            {
                // If the login was successful, add the picture claim to the user
                if (!string.IsNullOrEmpty(pictureClaim))
                {
                    var user = await _accountService.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                    if (user != null)
                    {
                        var claims = new List<Claim> { new Claim("picture", pictureClaim) };
                        await _accountService.AddClaimsAsync(user, claims);

                        // Refresh the user's sign-in to include the new claim
                        await _accountService.SignInAsync(user);
                    }
                }

                _logger.LogInformation($"Login success");
                // Redirect to the intended URL after successful login
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // Login with external provider fail, we need to check if user exists or not
                bool registerResult = await _accountService.RegisterUserIfNotExistAsync(info);

                if (!registerResult)
                {
                    _logger.LogError($"register result failed");
                    return RedirectToAction(nameof(Login));
                }

                // Add the picture claim after registration if the user was newly registered
                var newUser = await _accountService.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                if (newUser != null && !string.IsNullOrEmpty(pictureClaim))
                {
                    var claims = new List<Claim> { new Claim("picture", pictureClaim) };
                    await _accountService.AddClaimsAsync(newUser, claims);

                    // Refresh the user's sign-in to include the new claim
                    await _accountService.SignInAsync(newUser);
                }

                _logger.LogInformation($"Login success");
                return RedirectToLocal(returnUrl);
            }
        }

        // Helper method to redirect to a local URL or default
        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (returnUrl == null)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            return Redirect(returnUrl);
        }
    }
}
