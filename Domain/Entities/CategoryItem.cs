namespace Domain.Entities;

public class CategoryItem
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? PhotoUrl { get; set; }
    public string? ProductDescription { get; set; }

    public Category Category { get; set; } = null!;
    public ICollection<OptionGroup> OptionGroups { get; set; } = new List<OptionGroup>();
    public ICollection<Addon> Addons { get; set; } = new List<Addon>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<PromotionItem> PromotionItems { get; set; } = new List<PromotionItem>();
}
