using DrinkShop.Models;
using Microsoft.AspNetCore.Identity;

namespace DrinkShop.Data
{
    public class Database_Initializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceProvider>()
                .CreateScope())
            {
                User adminUser = null;
                User serviceUser = null;
                IdentityRole adminRole = null;
                IdentityRole serviceRole = null;
                var context = serviceScope.ServiceProvider.GetRequiredService<DrinkShopDbContext>();
                if (context != null)
                {
                    if (context.Users != null && !context.Users.Any())
                    {
                        adminUser = new User()
                        {
                            PasswordHash =
                                "AQAAAAEAACcQAAAAEE5W8z3JXjlDAENV/mrcVLZ8rlmSq3FzpNfatgjigHhfrvQPEMIjQRLNUYED5Nt9rQ==",
                            Name = "narges",
                            Family = "hashemi",
                            UserName = "Admin@gmail.com",
                            NormalizedUserName = "ADMIN@GMAIL.COM",
                            Email = "Admin@gmail.com",
                            NormalizedEmail = "ADMIN@GMAIL.COM",
                            EmailConfirmed = true,
                        };

                        context.Users.Add(adminUser);
                    };
                }
                if (context.Roles != null && !context.Roles.Any())
                {
                    adminRole = new IdentityRole()
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    };
                    var roles = new List<IdentityRole>()
                    {
                        adminRole,
                        new IdentityRole()
                        {
                            Name = "Buyer",
                            NormalizedName = "BUYER"
                        }
                    };
                    context.AddRange(roles);
                }
                if (adminUser != null && adminRole != null)
                {
                    context.UserRoles.Add(new IdentityUserRole<string>()
                    {
                        RoleId = adminRole.Id,
                        UserId = adminUser.Id
                    });
                }
                context.SaveChanges();
            }
        }
    }
}

