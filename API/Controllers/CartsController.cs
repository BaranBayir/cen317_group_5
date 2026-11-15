using System.Security.Claims;
using Application.DTOs.Carts;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/cart")]
[Authorize(Roles = RoleNames.Customer)]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class CartsController(ICartService cartService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var userId = GetUserId();

        if (userId is null)
            return Unauthorized();

        var cart = await cartService.GetCartAsync(userId.Value);

        return cart is null ? NotFound() : Ok(cart);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddItem(
        [FromBody] AddCartItemRequest request)
    {
        var userId = GetUserId();

        if (userId is null)
            return Unauthorized();

        var result = await cartService.AddItemAsync(userId.Value, request);

        return ToActionResult(result);
    }

    [HttpPut("items/{cartItemId:int:min(1)}")]
    public async Task<IActionResult> UpdateItem(
        int cartItemId,
        [FromBody] UpdateCartItemRequest request)
    {
        var userId = GetUserId();

        if (userId is null)
            return Unauthorized();

        var result = await cartService.UpdateItemAsync(
            userId.Value, cartItemId, request);

        return ToActionResult(result);
    }

    [HttpDelete("items/{cartItemId:int:min(1)}")]
    public async Task<IActionResult> RemoveItem(int cartItemId)
    {
        var userId = GetUserId();

        if (userId is null)
            return Unauthorized();

        var result = await cartService.RemoveItemAsync(
            userId.Value, cartItemId);

        return ToActionResult(result);
    }

    private int? GetUserId()
    {
        var value = User.FindFirstValue(ClaimTypes.NameIdentifier);

        return int.TryParse(value, out var userId) && userId > 0
            ? userId
            : null;
    }

    private IActionResult ToActionResult(CartResult result)
    {
        if (result.Succeeded)
            return Ok(result.Cart);

        return result.Failure switch
        {
            CartFailure.NotFound => NotFound(new { errors = result.Errors }),
            CartFailure.Conflict => Conflict(new { errors = result.Errors }),
            _ => BadRequest(new { errors = result.Errors })
        };
    }
}