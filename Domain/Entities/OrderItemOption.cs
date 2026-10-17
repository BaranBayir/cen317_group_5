namespace Domain.Entities;

public class OrderItemOption
{
    public int Id { get; set; }
    public int OrderItemId { get; set; }
    public int OptionId { get; set; }
    public decimal PriceModifier { get; set; }

    public OrderItem OrderItem { get; set; } = null!;
    public Option Option { get; set; } = null!;
}
