
using Application.DTOs.Carts;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CartService(AppDbContext dbContext) : ICartService
{
    public async Task<CartDto?> GetCartAsync(int userId)
    {
        var cartId = await dbContext.Carts
            .Where(c => c.UserId == userId)
            .OrderBy(c => c.Id)
            .Select(c => (int?)c.Id)
            .FirstOrDefaultAsync();

        return cartId is null
            ? null
            : await GetCartDtoAsync(cartId.Value);
    }

    public async Task<CartResult> AddItemAsync(
        int userId,
        AddCartItemRequest request)
    {
        if (request.Quantity < 1)
            return CartResult.Fail(
                CartFailure.Validation, "Quantity must be at least 1.");

        var categoryItem = await GetCategoryItemAsync(request.CategoryItemId);

        if (categoryItem is null)
            return CartResult.Fail(
                CartFailure.NotFound, "Product not found.");

        var optionIds = request.OptionIds ?? new List<int>();
        var addonIds = request.AddonIds ?? new List<int>();

        if (!TryValidateSelections(
            categoryItem, optionIds, addonIds,
            out var selectedOptions, out var selectedAddons,
            out var error))
        {
            return CartResult.Fail(CartFailure.Validation, error);
        }

        var restaurantId = categoryItem.Category.Menu.RestaurantId;

        var cart = await dbContext.Carts
            .Include(c => c.CartItems)
            .OrderBy(c => c.Id)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is not null &&
            cart.CartItems.Count > 0 &&
            cart.RestaurantId != restaurantId)
        {
            return CartResult.Fail(
                CartFailure.Conflict,
                "Your cart already contains products from another restaurant.");
        }

        if (cart is null)
        {
            cart = new Cart
            {
                UserId = userId,
                RestaurantId = restaurantId
            };

            dbContext.Carts.Add(cart);
        }
        else if (cart.CartItems.Count == 0)
        {
            cart.RestaurantId = restaurantId;
        }

        var cartItem = new CartItem
        {
            Cart = cart,
            CategoryItemId = categoryItem.Id,
            Quantity = request.Quantity
        };

        foreach (var option in selectedOptions)
        {
            cartItem.SelectedOptions.Add(new SelectedOption
            {
                OptionId = option.Id
            });
        }

        foreach (var addon in selectedAddons)
        {
            cartItem.SelectedAddons.Add(new SelectedAddon
            {
                AddonId = addon.Id
            });
        }

        cart.CartItems.Add(cartItem);

        await dbContext.SaveChangesAsync();

        return CartResult.Success(
            (await GetCartDtoAsync(cart.Id))!);
    }

    public async Task<CartResult> UpdateItemAsync(
        int userId,
        int cartItemId,
        UpdateCartItemRequest request)
    {
        if (request.Quantity < 1)
            return CartResult.Fail(
                CartFailure.Validation, "Quantity must be at least 1.");

        var cartItem = await dbContext.CartItems
            .Include(i => i.Cart)
            .Include(i => i.SelectedOptions)
            .Include(i => i.SelectedAddons)
            .FirstOrDefaultAsync(i =>
                i.Id == cartItemId && i.Cart.UserId == userId);

        if (cartItem is null)
            return CartResult.Fail(
                CartFailure.NotFound, "Cart item not found.");

        var categoryItem = await GetCategoryItemAsync(
            cartItem.CategoryItemId);

        if (categoryItem is null)
            return CartResult.Fail(
                CartFailure.NotFound, "Product not found.");

        var optionIds = request.OptionIds ?? new List<int>();
        var addonIds = request.AddonIds ?? new List<int>();

        if (!TryValidateSelections(
            categoryItem, optionIds, addonIds,
            out var selectedOptions, out var selectedAddons,
            out var error))
        {
            return CartResult.Fail(CartFailure.Validation, error);
        }

        dbContext.SelectedOptions.RemoveRange(
            cartItem.SelectedOptions.ToList());

        dbContext.SelectedAddons.RemoveRange(
            cartItem.SelectedAddons.ToList());

        cartItem.SelectedOptions.Clear();
        cartItem.SelectedAddons.Clear();
        cartItem.Quantity = request.Quantity;

        foreach (var option in selectedOptions)
        {
            cartItem.SelectedOptions.Add(new SelectedOption
            {
                OptionId = option.Id
            });
        }

        foreach (var addon in selectedAddons)
        {
            cartItem.SelectedAddons.Add(new SelectedAddon
            {
                AddonId = addon.Id
            });
        }

        await dbContext.SaveChangesAsync();

        return CartResult.Success(
            (await GetCartDtoAsync(cartItem.CartId))!);
    }

    public async Task<CartResult> RemoveItemAsync(
        int userId,
        int cartItemId)
    {
        var cartItem = await dbContext.CartItems
            .Include(i => i.Cart)
            .FirstOrDefaultAsync(i =>
                i.Id == cartItemId && i.Cart.UserId == userId);

        if (cartItem is null)
            return CartResult.Fail(
                CartFailure.NotFound, "Cart item not found.");

        var cartId = cartItem.CartId;

        dbContext.CartItems.Remove(cartItem);
        await dbContext.SaveChangesAsync();

        return CartResult.Success(
            (await GetCartDtoAsync(cartId))!);
    }

    private Task<CategoryItem?> GetCategoryItemAsync(int categoryItemId)
    {
        return dbContext.CategoryItems
            .AsNoTracking()
            .AsSplitQuery()
            .Include(i => i.Category)
                .ThenInclude(c => c.Menu)
            .Include(i => i.OptionGroups)
                .ThenInclude(g => g.Options)
            .Include(i => i.Addons)
            .SingleOrDefaultAsync(i => i.Id == categoryItemId);
    }

    private static bool TryValidateSelections(
        CategoryItem categoryItem,
        IReadOnlyCollection<int> optionIds,
        IReadOnlyCollection<int> addonIds,
        out List<Option> selectedOptions,
        out List<Addon> selectedAddons,
        out string error)
    {
        selectedOptions = new List<Option>();
        selectedAddons = new List<Addon>();
        error = string.Empty;

        if (optionIds.Count != optionIds.Distinct().Count())
        {
            error = "Duplicate options are not allowed.";
            return false;
        }

        if (addonIds.Count != addonIds.Distinct().Count())
        {
            error = "Duplicate add-ons are not allowed.";
            return false;
        }

        var availableOptions = categoryItem.OptionGroups
            .SelectMany(g => g.Options)
            .ToList();

        selectedOptions = availableOptions
            .Where(o => optionIds.Contains(o.Id))
            .ToList();

        if (selectedOptions.Count != optionIds.Count)
        {
            error = "One or more selected options do not belong to this product.";
            return false;
        }

        foreach (var group in categoryItem.OptionGroups)
        {
            var selectedCount = selectedOptions.Count(
                o => o.OptionGroupId == group.Id);

            var minimumRequired = group.IsRequired
                ? Math.Max(1, group.MinSelect)
                : group.MinSelect;

            if (selectedCount < minimumRequired)
            {
                error = $"The option group '{group.Name}' requires more selections.";
                return false;
            }

            if (selectedCount > group.MaxSelect)
            {
                error = $"Too many options selected in '{group.Name}'.";
                return false;
            }
        }

        selectedAddons = categoryItem.Addons
            .Where(a => addonIds.Contains(a.Id))
            .ToList();

        if (selectedAddons.Count != addonIds.Count)
        {
            error = "One or more selected add-ons do not belong to this product.";
            return false;
        }

        return true;
    }

    private async Task<CartDto?> GetCartDtoAsync(int cartId)
    {
        var cart = await dbContext.Carts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(c => c.Restaurant)
            .Include(c => c.CartItems)
                .ThenInclude(i => i.CategoryItem)
            .Include(c => c.CartItems)
                .ThenInclude(i => i.SelectedOptions)
                    .ThenInclude(o => o.Option)
            .Include(c => c.CartItems)
                .ThenInclude(i => i.SelectedAddons)
                    .ThenInclude(a => a.Addon)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart is null)
            return null;

        var items = cart.CartItems
            .OrderBy(i => i.Id)
            .Select(item =>
            {
                var basePrice = item.CategoryItem.Price;

                var optionTotal = item.SelectedOptions
                    .Sum(o => o.Option.PriceModifier);

                var addonTotal = item.SelectedAddons
                    .Sum(a => a.Addon.PriceModifier);

                var unitPrice = basePrice + optionTotal + addonTotal;

                return new CartItemDto
                {
                    Id = item.Id,
                    CategoryItemId = item.CategoryItemId,
                    ProductName = item.CategoryItem.Name,
                    Quantity = item.Quantity,
                    BasePrice = basePrice,
                    UnitPrice = unitPrice,
                    LineTotal = unitPrice * item.Quantity,

                    Options = item.SelectedOptions
                        .Select(o => new CartOptionDto
                        {
                            Id = o.Option.Id,
                            Name = o.Option.Name,
                            PriceModifier = o.Option.PriceModifier
                        })
                        .ToList(),

                    Addons = item.SelectedAddons
                        .Select(a => new CartAddonDto
                        {
                            Id = a.Addon.Id,
                            Name = a.Addon.Name,
                            PriceModifier = a.Addon.PriceModifier
                        })
                        .ToList()
                };
            })
            .ToList();

        return new CartDto
        {
            Id = cart.Id,
            RestaurantId = cart.RestaurantId,
            RestaurantName = cart.Restaurant.Name,
            Items = items,
            TotalAmount = items.Sum(i => i.LineTotal)
        };
    }
}
