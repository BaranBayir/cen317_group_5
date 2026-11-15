
namespace Application.DTOs.Restaurants;

public class RestaurantDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
}

public class RestaurantMenuDto
{
    public int RestaurantId { get; init; }
    public string RestaurantName { get; init; } = string.Empty;
    public IReadOnlyList<MenuCategoryDto> Categories { get; init; }
        = Array.Empty<MenuCategoryDto>();
}

public class MenuCategoryDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public IReadOnlyList<MenuItemDto> Items { get; init; }
        = Array.Empty<MenuItemDto>();
}

public class MenuItemDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string? PhotoUrl { get; init; }
    public string? ProductDescription { get; init; }

    public IReadOnlyList<OptionGroupDto> OptionGroups { get; init; }
        = Array.Empty<OptionGroupDto>();

    public IReadOnlyList<AddonDto> Addons { get; init; }
        = Array.Empty<AddonDto>();
}

public class OptionGroupDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsRequired { get; init; }
    public int MinSelect { get; init; }
    public int MaxSelect { get; init; }

    public IReadOnlyList<OptionDto> Options { get; init; }
        = Array.Empty<OptionDto>();
}

public class OptionDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal PriceModifier { get; init; }
}

public class AddonDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal PriceModifier { get; init; }
}
