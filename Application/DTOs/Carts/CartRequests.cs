
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Carts;

public class AddCartItemRequest
{
    [Range(1, int.MaxValue)]
    public int CategoryItemId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;

    public List<int> OptionIds { get; set; } = new();

    public List<int> AddonIds { get; set; } = new();
}

public class UpdateCartItemRequest
{
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; } = 1;

    public List<int> OptionIds { get; set; } = new();

    public List<int> AddonIds { get; set; } = new();
}
