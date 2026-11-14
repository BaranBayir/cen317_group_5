using Application.DTOs.Addresses;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AddressService(AppDbContext dbContext) : IAddressService
{
    public async Task<IReadOnlyList<AddressDto>> GetAllAsync(int userId)
    {
        var addresses = await dbContext.CustomerAddresses.AsNoTracking()
            .Where(address => address.UserId == userId)
            .OrderBy(address => address.Id)
            .ToListAsync();

        return addresses.Select(ToDto).ToList();
    }

    public async Task<AddressDto?> GetByIdAsync(int userId, int addressId)
    {
        var address = await dbContext.CustomerAddresses.AsNoTracking()
            .SingleOrDefaultAsync(address => address.Id == addressId && address.UserId == userId);

        return address is null ? null : ToDto(address);
    }

    public async Task<AddressDto> CreateAsync(int userId, AddressRequest request)
    {
        var address = new CustomerAddress
        {
            UserId = userId,
            Title = request.Title.Trim(),
            City = request.City.Trim(),
            District = request.District.Trim(),
            AddressLine = request.AddressLine.Trim()
        };

        dbContext.CustomerAddresses.Add(address);
        await dbContext.SaveChangesAsync();
        return ToDto(address);
    }

    public async Task<bool> UpdateAsync(int userId, int addressId, AddressRequest request)
    {
        var affectedRows = await dbContext.CustomerAddresses
            .Where(address => address.Id == addressId && address.UserId == userId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(address => address.Title, request.Title.Trim())
                .SetProperty(address => address.City, request.City.Trim())
                .SetProperty(address => address.District, request.District.Trim())
                .SetProperty(address => address.AddressLine, request.AddressLine.Trim()));

        return affectedRows == 1;
    }

    public async Task<bool> DeleteAsync(int userId, int addressId)
    {
        var affectedRows = await dbContext.CustomerAddresses
            .Where(address => address.Id == addressId && address.UserId == userId)
            .ExecuteDeleteAsync();

        return affectedRows == 1;
    }

    private static AddressDto ToDto(CustomerAddress address) => new()
    {
        Id = address.Id,
        Title = address.Title,
        City = address.City,
        District = address.District,
        AddressLine = address.AddressLine
    };
}
