namespace Domain.Entities;

public class Addon
{
    public int Id { get; set; }
    public int CategoryItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PriceModifier { get; set; }

    public CategoryItem CategoryItem { get; set; } = null!;
    public ICollection<SelectedAddon> SelectedAddons { get; set; } = new List<SelectedAddon>();
    public ICollection<OrderItemAddon> OrderItemAddons { get; set; } = new List<OrderItemAddon>();
}
