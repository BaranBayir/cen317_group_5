using Application.DTOs.Addresses;

namespace Application.Interfaces;

public interface IAddressService
{
    Task<IReadOnlyList<AddressDto>> GetAllAsync(int userId);
    Task<AddressDto?> GetByIdAsync(int userId, int addressId);
    Task<AddressDto> CreateAsync(int userId, AddressRequest request);
    Task<bool> UpdateAsync(int userId, int addressId, AddressRequest request);
    Task<bool> DeleteAsync(int userId, int addressId);
}
