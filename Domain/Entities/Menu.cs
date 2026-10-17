namespace Domain.Entities;

public class Menu
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }

    public Restaurant Restaurant { get; set; } = null!;
    public ICollection<Category> Categories { get; set; } = new List<Category>();
}
