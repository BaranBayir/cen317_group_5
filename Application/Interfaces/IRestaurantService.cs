
using Application.DTOs.Restaurants;

namespace Application.Interfaces;

public interface IRestaurantService
{
    Task<IReadOnlyList<RestaurantDto>> GetAllAsync();

    Task<RestaurantMenuDto?> GetMenuAsync(int restaurantId);
}
