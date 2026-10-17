namespace Domain.Entities;

public class Option
{
    public int Id { get; set; }
    public int OptionGroupId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal PriceModifier { get; set; }

    public OptionGroup OptionGroup { get; set; } = null!;
    public ICollection<SelectedOption> SelectedOptions { get; set; } = new List<SelectedOption>();
    public ICollection<OrderItemOption> OrderItemOptions { get; set; } = new List<OrderItemOption>();
}
