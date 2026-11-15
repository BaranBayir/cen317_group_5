
using Application.DTOs.Restaurants;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class RestaurantService(AppDbContext dbContext) : IRestaurantService
{
    public async Task<IReadOnlyList<RestaurantDto>> GetAllAsync()
    {
        return await dbContext.Restaurants
            .AsNoTracking()
            .OrderBy(restaurant => restaurant.Name)
            .Select(restaurant => new RestaurantDto
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Address = restaurant.Address,
                PhoneNumber = restaurant.PhoneNumber
            })
            .ToListAsync();
    }

    public async Task<RestaurantMenuDto?> GetMenuAsync(int restaurantId)
    {
        var restaurant = await dbContext.Restaurants
            .AsNoTracking()
            .AsSplitQuery()
            .Include(r => r.Menu)
                .ThenInclude(m => m.Categories)
                    .ThenInclude(c => c.CategoryItems)
                        .ThenInclude(i => i.OptionGroups)
                            .ThenInclude(g => g.Options)
            .Include(r => r.Menu)
                .ThenInclude(m => m.Categories)
                    .ThenInclude(c => c.CategoryItems)
                        .ThenInclude(i => i.Addons)
            .SingleOrDefaultAsync(r => r.Id == restaurantId);

        if (restaurant is null)
            return null;

        return new RestaurantMenuDto
        {
            RestaurantId = restaurant.Id,
            RestaurantName = restaurant.Name,

            Categories = restaurant.Menu?.Categories
                .OrderBy(c => c.Id)
                .Select(c => new MenuCategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,

                    Items = c.CategoryItems
                        .OrderBy(i => i.Id)
                        .Select(i => new MenuItemDto
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Price = i.Price,
                            PhotoUrl = i.PhotoUrl,
                            ProductDescription = i.ProductDescription,

                            OptionGroups = i.OptionGroups
                                .OrderBy(g => g.Id)
                                .Select(g => new OptionGroupDto
                                {
                                    Id = g.Id,
                                    Name = g.Name,
                                    IsRequired = g.IsRequired,
                                    MinSelect = g.MinSelect,
                                    MaxSelect = g.MaxSelect,

                                    Options = g.Options
                                        .OrderBy(o => o.Id)
                                        .Select(o => new OptionDto
                                        {
                                            Id = o.Id,
                                            Name = o.Name,
                                            PriceModifier = o.PriceModifier
                                        })
                                        .ToList()
                                })
                                .ToList(),

                            Addons = i.Addons
                                .OrderBy(a => a.Id)
                                .Select(a => new AddonDto
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    PriceModifier = a.PriceModifier
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList() ?? new List<MenuCategoryDto>()
        };
    }
}
