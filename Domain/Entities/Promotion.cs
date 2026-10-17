using Domain.Enums;

namespace Domain.Entities;

public class Promotion
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public PromotionAppliesTo AppliesTo { get; set; }
    public DiscountType DiscountType { get; set; }
    public decimal DiscountValue { get; set; }
    public decimal MinOrderAmount { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime EndAt { get; set; }
    public bool IsActive { get; set; } = true;
    public string? Code { get; set; }

    public Restaurant Restaurant { get; set; } = null!;
    public ICollection<PromotionItem> PromotionItems { get; set; } = new List<PromotionItem>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
