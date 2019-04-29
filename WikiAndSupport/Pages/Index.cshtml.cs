using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WikiAndSupport.Data;

namespace WikiAndSupport.Pages
{
    public class IndexModel : PageModel
    {
        public bool HasEasIssue => Issues.Count > 0;
        public IList<WikiAndSupport.Models.Issues.Issue> Issues { get; set; }
        private readonly IssueContext _db;
        public IndexModel(IssueContext dbContext)
        {
            _db = dbContext;        

        }
        public async Task OnGetAsync()
        {
            if(_db!=null && _db.Issue!=null)
                Issues = await _db.Issue.OrderByDescending(m=>m.CreatedOn).ToListAsync();
            
        }
    }
}
