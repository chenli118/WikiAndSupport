using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using WikiAndSupport.Data;
using WikiAndSupport.Models.Issues;
using Microsoft.AspNetCore.Authorization;

namespace WikiAndSupport.Pages
{
    public class NewAnswerModel : PageModel
    {
        private readonly WikiAndSupport.Data.IssueContext _context;

        [TempData]
        public string Message { get; set; }

        public Issue Issue { get; set; }

        public int IssueID { get; set; }
        public NewAnswerModel(WikiAndSupport.Data.IssueContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            IssueID = int.Parse(Request.Query["id"]);
           
            Issue = await _context.Issue.Where(m => m.IssueId == IssueID).FirstOrDefaultAsync();            
            return Page();
            
        }

        [BindProperty]
        public Answer Answer { get; set; } 
        public async Task<IActionResult> OnPostAsync()
        {
            
            IssueID = int.Parse(Request.Query["id"]);
            Issue = await _context.Issue.Where(m => m.IssueId == IssueID).FirstOrDefaultAsync();
            Answer.Issue = Issue;
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
            Answer.Name = System.Text.Encoding.UTF8.GetString(result);
            Answer.Email = Answer.Name;
            Answer.CreatedBy = Answer.Name;
            Answer.CreatedOn = DateTime.Now;
            _context.Answer.Add(Answer);
            await _context.SaveChangesAsync();

            return RedirectToPage("./IssueDetail",new { id = Request.Query["id"] });
        }
    }
}