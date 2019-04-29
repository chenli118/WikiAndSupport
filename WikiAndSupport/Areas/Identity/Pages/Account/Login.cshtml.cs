using System;

using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NMAccountHelper;
using WikiAndSupport.Data;
using Microsoft.Extensions.Configuration;

namespace WikiAndSupport.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        public LoginModel(SignInManager<IdentityUser> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }
               
        [BindProperty]
        public InputModel Input { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        public class InputModel
        {
            [Required]            
            [Display(Name = "账号")]
            public string Email { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "密码")]
            public string Password { get; set; }
            [Display(Name = "保存密码")]
            public bool RememberMe { get; set; }

        }



        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }
            returnUrl = returnUrl ?? Url.Content("~/");
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ReturnUrl = returnUrl;
        }
        #region snippet

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var builder = new ConfigurationBuilder().AddJsonFile("WebApp.Config.json", optional: false, reloadOnChange: true);
                    var configure=builder.Build();
                var config = configure.GetConnectionString("NMAccoutViewDBConnect");
                bool nmLogin=
                    NMAccountHelper.NMAccount.Login(config, Input.Email, Input.Password);
                if (!nmLogin)
                {
                    ModelState.AddModelError(string.Empty, "办公系统认证失败.");
                    return Page();
                }
                else
                {
                    HttpContext.Session.Remove("Secr");
                    HttpContext.Session.Set("Secr", System.Text.Encoding.UTF8.GetBytes(Input.Password));
                    HttpContext.Session.Remove("CurrentUser");
                    HttpContext.Session.Set("CurrentUser", System.Text.Encoding.UTF8.GetBytes(Input.Email));
                    return RedirectToPage("./Register", new { ReturnUrl = returnUrl, usecode = Input.Email, usepwd = Input.Password, });
                }


                    var result = await _signInManager.PasswordSignInAsync(Input.Email,
                    Input.Password, Input.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    byte[] bUserName = System.Text.Encoding.UTF8.GetBytes(Input.Email);
                    HttpContext.Session.Remove("CurrentUser");
                    HttpContext.Session.Set("CurrentUser", bUserName);
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }
            // If we got this far, something failed, redisplay form
            return Page();
        }

        #endregion

    }

}