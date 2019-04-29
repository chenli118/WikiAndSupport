using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiAndSupport.Models;
using WikiAndSupport.Models.Issues;

namespace WikiAndSupport.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Users.Any())
                return;
            var salt = Guid.NewGuid().ToString();
            IdentityUser admin = new IdentityUser
            {
                UserName = "Admin",
                Email = "Admin@Eas.Com",
                PasswordHash = "AQAAAAEAACcQAAAAEPg9D/xy7ijIp6kcftc4m+0yhAPMRY1qHdAu/27vFyncoKmibTS57FA/5E5sm0D7Hw=="
            };

            context.Users.Add(admin);
            context.SaveChanges();
        }
        public static void Initialize2(IssueContext context)
        {
            context.Database.EnsureCreated();
            if (!context.Issue.Any())
            {
                Issue issue = new Issue
                {
                    IssueId = 1,
                    Title = "默认",
                    CreatedBy = "System",
                    CreatedOn = DateTime.Now
                };
                context.Issue.Add(issue);
            }

            context.SaveChanges();
        }
    }
}
