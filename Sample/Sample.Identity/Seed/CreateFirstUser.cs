using Microsoft.AspNetCore.Identity;

namespace Sample.Identity.Seed
{
    public static class UserCreator
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var applicationUser = new ApplicationUser
            {
                FirstName = "Lorem ipslum",
                LastName = "dolor",
                UserName = "userTest",
                Email = "lorem@hotmail.com",
                EmailConfirmed = true
            };

            var user = await userManager.FindByEmailAsync(applicationUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(applicationUser, "dev@123");
            }
        }
    }
}