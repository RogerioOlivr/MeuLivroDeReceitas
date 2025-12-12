using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.API.Filters;

public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _accessTokenValidator;
    private readonly IUserReadOnlyRepository _repository;
    private readonly ILogger<AuthenticatedUserFilter> _logger;

    public AuthenticatedUserFilter(
        IAccessTokenValidator accessTokenValidator, 
        IUserReadOnlyRepository repository,
        ILogger<AuthenticatedUserFilter> logger)
    {
        _accessTokenValidator = accessTokenValidator;
        _repository = repository;
        _logger = logger;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            _logger.LogInformation("Iniciando validação de autenticação...");
            
            var token = TokenOnRequest(context);
            _logger.LogInformation("Token extraído do header");

            var userIdentifier = _accessTokenValidator.ValidateAndGetUserIdentifier(token);
            _logger.LogInformation("Token validado. UserIdentifier: {UserIdentifier}", userIdentifier);

            var exist = await _repository.ExistActiveUserWithIdentifier(userIdentifier);
            _logger.LogInformation("Verificação no banco concluída. Usuário existe: {Exist}", exist);
            
            if (exist.IsFalse())
            {
                _logger.LogWarning("Usuário não encontrado ou inativo. UserIdentifier: {UserIdentifier}", userIdentifier);
                throw new UnauthorizedException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
            }
            
            _logger.LogInformation("Autenticação bem-sucedida");
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("TokenIsExpired")
            {
                TokenIsExpired = true,
            });
        }
        catch (SecurityTokenException)
        {
            // Token inválido ou malformado
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE));
        }
        catch (MyRecipeBookException myRecipeBookException)
        {
            context.HttpContext.Response.StatusCode = (int)myRecipeBookException.GetStatusCode();
            context.Result = new ObjectResult(new ResponseErrorJson(myRecipeBookException.GetErrorMessages()));
        }
        catch (Exception ex)
        {
            // Log detalhado da exceção
            _logger.LogError(ex, "Erro na autenticação do usuário. Tipo: {ExceptionType}, Mensagem: {Message}", 
                ex.GetType().Name, ex.Message);
            
            Console.WriteLine($"=== ERRO NA AUTENTICAÇÃO ===");
            Console.WriteLine($"Tipo: {ex.GetType().Name}");
            Console.WriteLine($"Mensagem: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException.GetType().Name} - {ex.InnerException.Message}");
            }
            Console.WriteLine($"============================");
            
            context.Result = new UnauthorizedObjectResult(new ResponseErrorJson(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE));
        }
    }

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authentication))
        {
            throw new UnauthorizedException(ResourceMessagesException.NO_TOKEN);
        }

        // Remove "Bearer " se estiver presente
        if (authentication.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return authentication["Bearer ".Length..].Trim();
        }

        return authentication.Trim();
    }
}
