using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WikiAndSupport.Areas.Identity.Pages.Account
{

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;



        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }
               
        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public class InputModel

        {

            [Required] 
            [Display(Name = "账号")]
            public string Email { get; set; }

            [Required]
            //[StringLength(100, ErrorMessage = "这最少 {0} 需要 {2} 最大需要 {1} 的字符长度.", MinimumLength = 6)]

            [DataType(DataType.Password)]

            [Display(Name = "密码")]
            public string Password { get; set; }

            [DataType(DataType.Password)]

            [Display(Name = "再确认一次密码")]

            [Compare("Password", ErrorMessage = "两次密码不匹配")]

            public string ConfirmPassword { get; set; }

        }



        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            byte[] uname = null;
            byte[] upwd = null;
            HttpContext.Session.TryGetValue("CurrentUser",out uname);
            HttpContext.Session.TryGetValue("Secr", out upwd);
            if (uname == null || upwd == null) return Page();
            string uid = System.Text.Encoding.UTF8.GetString(uname);
            string pwd = System.Text.Encoding.UTF8.GetString(upwd);
            var user = new IdentityUser { UserName = uid, Email = uid };
            var result = await _userManager.CreateAsync(user, pwd);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");                  
                await _signInManager.SignInAsync(user, isPersistent: false);                
            }
            else
            { 
                   var ret=   await _signInManager.PasswordSignInAsync(uid,pwd,true,
                   lockoutOnFailure: true);
                if (!ret.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }
            }
            return LocalRedirect(returnUrl);
        }



        #region snippet

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    byte[] bUserName = System.Text.Encoding.UTF8.GetBytes(Input.Email);
                    HttpContext.Session.Remove("CurrentUser");
                    HttpContext.Session.Set("CurrentUser", bUserName);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }            
            // If we got this far, something failed, redisplay form

            return Page();

        }

        #endregion

    }

}