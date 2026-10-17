namespace Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public int MenuId { get; set; }
    public string Name { get; set; } = string.Empty;

    public Menu Menu { get; set; } = null!;
    public ICollection<CategoryItem> CategoryItems { get; set; } = new List<CategoryItem>();
}
