namespace Domain.Entities;

public class PromotionItem
{
    public int PromotionId { get; set; }
    public int CategoryItemId { get; set; }

    public Promotion Promotion { get; set; } = null!;
    public CategoryItem CategoryItem { get; set; } = null!;
}
