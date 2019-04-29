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
    public class IssueListModel : PageModel
    {
        private readonly WikiAndSupport.Data.IssueContext _context;

        public IssueListModel(WikiAndSupport.Data.IssueContext context)
        {
            _context = context;
        }

        public IList<Issue> Issue { get;set; }

        public async Task OnGetAsync()
        {
            Issue = await _context.Issue.ToListAsync();
        }
    }
}
