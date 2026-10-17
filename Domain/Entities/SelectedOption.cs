namespace Domain.Entities;

public class SelectedOption
{
    public int Id { get; set; }
    public int CartItemId { get; set; }
    public int OptionId { get; set; }

    public CartItem CartItem { get; set; } = null!;
    public Option Option { get; set; } = null!;
}
