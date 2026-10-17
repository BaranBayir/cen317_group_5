namespace Domain.Entities;

public class OptionGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CategoryItemId { get; set; }
    public bool IsRequired { get; set; }
    public int MinSelect { get; set; }
    public int MaxSelect { get; set; } = 1;

    public CategoryItem CategoryItem { get; set; } = null!;
    public ICollection<Option> Options { get; set; } = new List<Option>();
}
