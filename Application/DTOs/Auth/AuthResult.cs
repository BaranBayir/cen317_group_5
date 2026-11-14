namespace Application.DTOs.Auth;

public enum AuthFailure
{
    None,
    Validation,
    Conflict,
    InvalidCredentials
}

public class AuthResult
{
    public bool Succeeded => Failure == AuthFailure.None;
    public AuthFailure Failure { get; init; }
    public AuthUserDto? User { get; init; }
    public string[] Errors { get; init; } = [];

    public static AuthResult Success(AuthUserDto user) => new() { User = user };

    public static AuthResult Fail(AuthFailure failure, params string[] errors) =>
        new() { Failure = failure, Errors = errors };
}
