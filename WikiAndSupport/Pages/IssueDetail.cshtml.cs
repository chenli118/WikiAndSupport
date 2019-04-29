using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WikiAndSupport.Data;
using WikiAndSupport.Models.Issues;

namespace WikiAndSupport.Pages
{
    public class IssueDetailModel : PageModel
    {
        private readonly WikiAndSupport.Data.IssueContext _context;

        public IssueDetailModel(WikiAndSupport.Data.IssueContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Issue Issue { get; set; }

        public IList<Answer> Answers { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Issue = await _context.Issue.FirstOrDefaultAsync(m => m.IssueId == id);
            Answers = await _context.Answer.Where(a => a.Issue == Issue).ToListAsync();
            if (Issue == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
