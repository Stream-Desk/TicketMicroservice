using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Microsoft.AspNetCore.Identity;

namespace API.Data
{
    public static class DbSeeder
    {
        public static void SeedDb(UserManager<IdentityUser> userManager)
        {
            

            IdentityUser user = new IdentityUser
            {
                UserName = "cathyAkoth",
                Email = "cathy@gmail.com"
            };

            userManager.CreateAsync(user, "Password1234!").Wait();
        }
    }
}
