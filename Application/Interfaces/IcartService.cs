
using Application.DTOs.Carts;

namespace Application.Interfaces;

public interface ICartService
{
    Task<CartDto?> GetCartAsync(int userId);

    Task<CartResult> AddItemAsync(
        int userId,
        AddCartItemRequest request);

    Task<CartResult> UpdateItemAsync(
        int userId,
        int cartItemId,
        UpdateCartItemRequest request);

    Task<CartResult> RemoveItemAsync(
        int userId,
        int cartItemId);
}
