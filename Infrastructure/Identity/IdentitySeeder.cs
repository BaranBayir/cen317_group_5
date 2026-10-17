using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole<int>> roleManager)
    {
        string[] roleNames =
        {
            RoleNames.Customer,
            RoleNames.RestaurantOwner,
            RoleNames.Courier,
            RoleNames.Admin
        };

        foreach (var roleName in roleNames)
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                continue;
            }

            var result = await roleManager.CreateAsync(new IdentityRole<int>(roleName));

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(error => error.Description));
                throw new InvalidOperationException($"Could not create role '{roleName}': {errors}");
            }
        }
    }
}
