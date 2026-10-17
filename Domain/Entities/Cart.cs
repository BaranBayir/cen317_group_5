namespace Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RestaurantId { get; set; }

    public Restaurant Restaurant { get; set; } = null!;
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
