using Microsoft.AspNetCore.Identity;
using WebApplication1.Modules.Users.Entities;

namespace WebApplication1.Modules.Users.Seeders
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<Role> roleManager)
        {
            // Define core system roles
            var roles = new List<string> { "super-admin", "admin", "user" };

            foreach (var roleName in roles)
            {
                // Check if the role already exists in SQL Server
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    var newRole = new Role
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper(),
                        Description = $"Base role for {roleName} users.",
                        CreatedAt = DateTime.UtcNow,
                        // Initializes as an empty list, stored as "[]" via DbContext value converter
                        Permissions = new List<string>()
                    };

                    await roleManager.CreateAsync(newRole);
                }
            }
        }
    }
}