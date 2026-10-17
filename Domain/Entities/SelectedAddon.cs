namespace Domain.Entities;

public class SelectedAddon
{
    public int Id { get; set; }
    public int CartItemId { get; set; }
    public int AddonId { get; set; }

    public CartItem CartItem { get; set; } = null!;
    public Addon Addon { get; set; } = null!;
}
