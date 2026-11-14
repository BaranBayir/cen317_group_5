using Application.DTOs.Auth;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    AppDbContext dbContext) : IAuthService
{
    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim();

        if (await userManager.FindByEmailAsync(email) is not null)
        {
            return AuthResult.Fail(AuthFailure.Conflict, "This email is already registered.");
        }

        var user = new ApplicationUser
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            UserName = email
        };

        // User, role assignment and optional address are saved in the same transaction.
        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        try
        {
            var createResult = await userManager.CreateAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                var failure = createResult.Errors.Any(error =>
                    error.Code is "DuplicateEmail" or "DuplicateUserName")
                    ? AuthFailure.Conflict
                    : AuthFailure.Validation;

                return AuthResult.Fail(failure,
                    createResult.Errors.Select(error => error.Description).ToArray());
            }

            var roleResult = await userManager.AddToRoleAsync(user, RoleNames.Customer);
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException("Could not assign the customer role.");
            }

            if (request.Address is not null)
            {
                dbContext.CustomerAddresses.Add(new CustomerAddress
                {
                    UserId = user.Id,
                    Title = request.Address.Title.Trim(),
                    City = request.Address.City.Trim(),
                    District = request.Address.District.Trim(),
                    AddressLine = request.Address.AddressLine.Trim()
                });

                await dbContext.SaveChangesAsync();
            }

            var response = await ToUserDtoAsync(user);
            await transaction.CommitAsync();
            return AuthResult.Success(response);
        }
        catch (DbUpdateException exception) when
            (exception.InnerException is SqlException { Number: 2601 or 2627 })
        {
            await transaction.RollbackAsync();
            return AuthResult.Fail(AuthFailure.Conflict, "This email is already registered.");
        }
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email.Trim());
        if (user is null)
        {
            return InvalidLogin();
        }

        var result = await signInManager.PasswordSignInAsync(
            user, request.Password, request.RememberMe, lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            return InvalidLogin();
        }

        return AuthResult.Success(await ToUserDtoAsync(user));
    }

    public Task LogoutAsync() => signInManager.SignOutAsync();

    public async Task<AuthUserDto?> GetUserAsync(int userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        return user is null ? null : await ToUserDtoAsync(user);
    }

    private async Task<AuthUserDto> ToUserDtoAsync(ApplicationUser user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email!,
        Roles = (await userManager.GetRolesAsync(user)).ToArray()
    };

    private static AuthResult InvalidLogin() => AuthResult.Fail(
        AuthFailure.InvalidCredentials,
        "Invalid email or password, or the account is temporarily unavailable.");
}
