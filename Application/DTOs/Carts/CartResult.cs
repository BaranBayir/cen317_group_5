
namespace Application.DTOs.Carts;

public enum CartFailure
{
    NotFound,
    Conflict,
    Validation
}

public class CartResult
{
    public bool Succeeded { get; init; }
    public CartDto? Cart { get; init; }
    public CartFailure? Failure { get; init; }

    public IReadOnlyList<string> Errors { get; init; }
        = Array.Empty<string>();

    public static CartResult Success(CartDto cart) => new()
    {
        Succeeded = true,
        Cart = cart
    };

    public static CartResult Fail(
        CartFailure failure,
        params string[] errors) => new()
        {
            Succeeded = false,
            Failure = failure,
            Errors = errors
        };
}
