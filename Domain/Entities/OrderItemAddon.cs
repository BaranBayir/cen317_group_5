namespace Domain.Entities;

public class OrderItemAddon
{
    public int Id { get; set; }
    public int OrderItemId { get; set; }
    public int AddonId { get; set; }
    public decimal PriceModifier { get; set; }

    public OrderItem OrderItem { get; set; } = null!;
    public Addon Addon { get; set; } = null!;
}
