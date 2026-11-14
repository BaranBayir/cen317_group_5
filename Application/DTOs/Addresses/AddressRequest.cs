using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Addresses;

public class AddressRequest
{
    [Required, StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string City { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string District { get; set; } = string.Empty;

    [Required, StringLength(1000)]
    public string AddressLine { get; set; } = string.Empty;
}
