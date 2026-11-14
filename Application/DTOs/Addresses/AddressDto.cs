namespace Application.DTOs.Addresses;

public class AddressDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string District { get; init; } = string.Empty;
    public string AddressLine { get; init; } = string.Empty;
}
