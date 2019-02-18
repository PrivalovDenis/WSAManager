
using WSAManager.Core.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace WSAManager.Data
{
    public class BigBangInitializer : DropCreateDatabaseIfModelChanges<AppDbContext>
    {
        protected override void Seed(AppDbContext context)
        {
            Initialize(context);
            base.Seed(context);
        }

        public void Initialize(AppDbContext context)
        {
            try
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.UserValidator = new UserValidator<ApplicationUser>(userManager)
                {
                    AllowOnlyAlphanumericUserNames = false
                };
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                if (!roleManager.RoleExists("Admin"))
                {
                    roleManager.Create(new IdentityRole("Admin"));
                }

                if (!roleManager.RoleExists("Member"))
                {
                    roleManager.Create(new IdentityRole("Member"));
                }

                var user = new ApplicationUser()
                {
                    Email = "wsa@email.co.uk",
                    UserName = "wsa@email.co.uk",
                    FirstName = "First Name",
                    LastName = "Last Name"
                };

                var userResult = userManager.Create(user, "admin");

                if (userResult.Succeeded)
                {
                    userManager.AddToRole<ApplicationUser, string>(user.Id, "Admin");
                }

                var prods = new List<Product>()
                {
                    new Product(){ Name = "Product #1" },
                    new Product(){ Name = "Product #2" },
                    new Product(){ Name = "Product #3" },
                    new Product(){ Name = "Product #4" },
                    new Product(){ Name = "Product #5" },
                };

                foreach (var prod in prods)
                {
                    context.Products.Add(prod);
                }

                context.Commit();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
