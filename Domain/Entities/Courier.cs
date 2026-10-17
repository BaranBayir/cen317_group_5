namespace Domain.Entities;

public class Courier
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool IsAvailable { get; set; } = true;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
