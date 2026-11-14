using System.Security.Claims;
using Application.DTOs.Addresses;
using Application.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/addresses")]
[Authorize(Roles = RoleNames.Customer)]
[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
public class AddressesController(IAddressService addressService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        return Ok(await addressService.GetAllAsync(userId));
    }

    [HttpGet("{id:int:min(1)}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        var address = await addressService.GetByIdAsync(userId, id);
        return address is null ? NotFound() : Ok(address);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AddressRequest request)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        var address = await addressService.CreateAsync(userId, request);
        return CreatedAtAction(nameof(GetById), new { id = address.Id }, address);
    }

    [HttpPut("{id:int:min(1)}")]
    public async Task<IActionResult> Update(int id, AddressRequest request)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        return await addressService.UpdateAsync(userId, id, request) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int:min(1)}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!TryGetUserId(out var userId)) return Unauthorized();

        return await addressService.DeleteAsync(userId, id) ? NoContent() : NotFound();
    }

    private bool TryGetUserId(out int userId) =>
        int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId) && userId > 0;
}
