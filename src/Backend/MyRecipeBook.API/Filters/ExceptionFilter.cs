using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exceptions;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if(context.Exception is MyRecipeBookException myRecipeBookException)
            HandleProjectException(myRecipeBookException, context);
        else
            ThrowUnknowException(context);
    }

    private static void HandleProjectException(MyRecipeBookException myRecipeBookException, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)myRecipeBookException.GetStatusCode();
        context.Result = new ObjectResult(new ResponseErrorJson(myRecipeBookException.GetErrorMessages()));
    }

    private void ThrowUnknowException(ExceptionContext context)
    {
        // Log detalhado da exceção
        _logger.LogError(context.Exception, 
            "Erro não tratado. Tipo: {ExceptionType}, Mensagem: {Message}", 
            context.Exception.GetType().Name, 
            context.Exception.Message);
        
        Console.WriteLine($"=== ERRO NÃO TRATADO ===");
        Console.WriteLine($"Tipo: {context.Exception.GetType().Name}");
        Console.WriteLine($"Mensagem: {context.Exception.Message}");
        Console.WriteLine($"Stack trace: {context.Exception.StackTrace}");
        if (context.Exception.InnerException != null)
        {
            Console.WriteLine($"Inner exception: {context.Exception.InnerException.GetType().Name}");
            Console.WriteLine($"Inner mensagem: {context.Exception.InnerException.Message}");
            Console.WriteLine($"Inner stack trace: {context.Exception.InnerException.StackTrace}");
        }
        Console.WriteLine($"========================");
        
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson(ResourceMessagesException.UNKNOWN_ERROR));
    }
}
