
namespace Application.DTOs.Carts;

public class CartDto
{
    public int Id { get; init; }
    public int RestaurantId { get; init; }
    public string RestaurantName { get; init; } = string.Empty;

    public IReadOnlyList<CartItemDto> Items { get; init; }
        = Array.Empty<CartItemDto>();

    public decimal TotalAmount { get; init; }
}

public class CartItemDto
{
    public int Id { get; init; }
    public int CategoryItemId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public int Quantity { get; init; }

    public decimal BasePrice { get; init; }
    public decimal UnitPrice { get; init; }
    public decimal LineTotal { get; init; }

    public IReadOnlyList<CartOptionDto> Options { get; init; }
        = Array.Empty<CartOptionDto>();

    public IReadOnlyList<CartAddonDto> Addons { get; init; }
        = Array.Empty<CartAddonDto>();
}

public class CartOptionDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal PriceModifier { get; init; }
}

public class CartAddonDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal PriceModifier { get; init; }
}
