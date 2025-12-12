using MyRecipeBook.Domain.Dtos;
using MyRecipeBook.Domain.Services.OpenAI;

namespace MyRecipeBook.Infrastructure.Services.OpenAI;

public class NullGenerateRecipeAI : IGenerateRecipeAI
{
    public Task<GeneratedRecipeDto> Generate(IList<string> ingredients)
    {
        return Task.FromResult(new GeneratedRecipeDto
        {
            Title = string.Empty,
            CookingTime = Domain.Enums.CookingTime.Less_10_Minutes,
            Ingredients = Array.Empty<string>(),
            Instructions = Array.Empty<GeneratedInstructionDto>()
        });
    }
}

