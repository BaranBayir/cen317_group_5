
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/restaurants")]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class RestaurantsController(IRestaurantService restaurantService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await restaurantService.GetAllAsync();
        return Ok(restaurants);
    }

    [HttpGet("{id:int:min(1)}/menu")]
    public async Task<IActionResult> GetMenu(int id)
    {
        var menu = await restaurantService.GetMenuAsync(id);

        return menu is null ? NotFound() : Ok(menu);
    }
}
