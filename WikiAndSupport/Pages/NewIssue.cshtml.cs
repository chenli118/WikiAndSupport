using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WikiAndSupport.Data;
using WikiAndSupport.Models.Issues;

namespace WikiAndSupport.Pages
{
    public class NewIssueModel : PageModel
    {
        private readonly WikiAndSupport.Data.IssueContext _context;

        public NewIssueModel(WikiAndSupport.Data.IssueContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        [TempData]
        public string Message { get; set; }
        [BindProperty]
        public Issue Issue { get; set; }
        [Authorize]
        public async Task<IActionResult> OnPostAsync([FromForm]Issue issue)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            byte[] result;
            HttpContext.Session.TryGetValue("CurrentUser", out result);
            if (result == null)
            {
                ModelState.AddModelError("ULogin", "需要登录才能继续操作！");
                Message = "需要登录才能继续操作！";
                return Page();
            }
            issue.CommitBy = System.Text.Encoding.UTF8.GetString(result);
            issue.CommitOn = DateTime.Now;
            issue.CreatedBy = issue.CommitBy;
            issue.CreatedOn = DateTime.Now;
            _context.Issue.Add(issue);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}