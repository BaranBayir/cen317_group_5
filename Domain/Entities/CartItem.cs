namespace Domain.Entities;

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int CategoryItemId { get; set; }
    public int Quantity { get; set; } = 1;

    public Cart Cart { get; set; } = null!;
    public CategoryItem CategoryItem { get; set; } = null!;
    public ICollection<SelectedOption> SelectedOptions { get; set; } = new List<SelectedOption>();
    public ICollection<SelectedAddon> SelectedAddons { get; set; } = new List<SelectedAddon>();
}
