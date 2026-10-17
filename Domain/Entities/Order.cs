using Domain.Enums;

namespace Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public int RestaurantId { get; set; }
    public int UserId { get; set; }
    public int? CourierId { get; set; }
    public int? PromotionId { get; set; }
    public decimal TotalAmount { get; set; }
    public string DeliveryAddress { get; set; } = string.Empty;

    public Restaurant Restaurant { get; set; } = null!;
    public Courier? Courier { get; set; }
    public Promotion? Promotion { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();
}
