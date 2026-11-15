
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class SampleDataSeeder
{
    public static async Task SeedAsync(
        UserManager<ApplicationUser> userManager,
        AppDbContext dbContext)
    {
        await EnsureRestaurantAsync(
            userManager,
            dbContext,
            "demo.owner@example.test",
            "Demo",
            "Owner",
            "Demo Burger House",
            "Örnek adres, Adana",
            "0000000000",
            new Menu
            {
                Categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Burgerler",
                        CategoryItems = new List<CategoryItem>
                        {
                            new CategoryItem
                            {
                                Name = "Klasik Burger",
                                Price = 180m,
                                ProductDescription = "Klasik hamburger",
                                OptionGroups = new List<OptionGroup>
                                {
                                    new OptionGroup
                                    {
                                        Name = "Boyut",
                                        IsRequired = true,
                                        MinSelect = 1,
                                        MaxSelect = 1,
                                        Options = new List<Option>
                                        {
                                            new Option
                                            {
                                                Name = "Normal",
                                                PriceModifier = 0m
                                            },
                                            new Option
                                            {
                                                Name = "Büyük",
                                                PriceModifier = 35m
                                            }
                                        }
                                    }
                                },
                                Addons = new List<Addon>
                                {
                                    new Addon
                                    {
                                        Name = "Ekstra Peynir",
                                        PriceModifier = 20m
                                    },
                                    new Addon
                                    {
                                        Name = "Ekstra Köfte",
                                        PriceModifier = 65m
                                    }
                                }
                            },
                            new CategoryItem
                            {
                                Name = "Tavuk Burger",
                                Price = 155m,
                                ProductDescription = "Çıtır tavuk burger"
                            }
                        }
                    },
                    new Category
                    {
                        Name = "İçecekler",
                        CategoryItems = new List<CategoryItem>
                        {
                            new CategoryItem
                            {
                                Name = "Kola",
                                Price = 40m
                            },
                            new CategoryItem
                            {
                                Name = "Ayran",
                                Price = 25m
                            }
                        }
                    }
                }
            });

        await EnsureRestaurantAsync(
            userManager,
            dbContext,
            "demo.owner2@example.test",
            "Demo",
            "Owner Two",
            "Demo Pizza House",
            "Örnek adres, Adana",
            "0000000001",
            new Menu
            {
                Categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Pizzalar",
                        CategoryItems = new List<CategoryItem>
                        {
                            new CategoryItem
                            {
                                Name = "Margherita Pizza",
                                Price = 220m,
                                ProductDescription = "Domates sosu ve peynirli pizza"
                            }
                        }
                    }
                }
            });
    }

    private static async Task EnsureRestaurantAsync(
        UserManager<ApplicationUser> userManager,
        AppDbContext dbContext,
        string ownerEmail,
        string firstName,
        string lastName,
        string restaurantName,
        string address,
        string phoneNumber,
        Menu menu)
    {
        var owner = await userManager.FindByEmailAsync(ownerEmail);

        if (owner is null)
        {
            owner = new ApplicationUser
            {
                UserName = ownerEmail,
                Email = ownerEmail,
                FirstName = firstName,
                LastName = lastName
            };

            var createResult = await userManager.CreateAsync(owner);

            if (!createResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    createResult.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"Demo owner could not be created: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(
            owner, RoleNames.RestaurantOwner))
        {
            var roleResult = await userManager.AddToRoleAsync(
                owner, RoleNames.RestaurantOwner);

            if (!roleResult.Succeeded)
            {
                var errors = string.Join(
                    ", ",
                    roleResult.Errors.Select(e => e.Description));

                throw new InvalidOperationException(
                    $"Demo owner role could not be assigned: {errors}");
            }
        }

        var restaurantExists = await dbContext.Restaurants
            .AnyAsync(r => r.UserId == owner.Id);

        if (restaurantExists)
            return;

        dbContext.Restaurants.Add(new Restaurant
        {
            Name = restaurantName,
            UserId = owner.Id,
            Address = address,
            PhoneNumber = phoneNumber,
            Menu = menu
        });

        await dbContext.SaveChangesAsync();
    }
}
