using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.API.Token;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpContextTokenValue(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string Value()
    {
        var authentication = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        // Remove "Bearer " se estiver presente
        if (authentication.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authentication["Bearer ".Length..].Trim();
        }

        return authentication.Trim();
    }
}
