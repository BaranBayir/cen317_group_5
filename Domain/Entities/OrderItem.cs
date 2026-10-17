namespace Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int CategoryItemId { get; set; }
    public int Quantity { get; set; } = 1;
    public decimal UnitPrice { get; set; }
    public int? PromotionId { get; set; }
    public decimal DiscountAmount { get; set; }

    public Order Order { get; set; } = null!;
    public CategoryItem CategoryItem { get; set; } = null!;
    public Promotion? Promotion { get; set; }
    public ICollection<OrderItemOption> OrderItemOptions { get; set; } = new List<OrderItemOption>();
    public ICollection<OrderItemAddon> OrderItemAddons { get; set; } = new List<OrderItemAddon>();
}
