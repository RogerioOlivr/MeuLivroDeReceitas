using MyRecipeBook.Domain.Services.ServiceBus;
using MyRecipeBook.Domain.Entities;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastructure.Services.ServiceBus;

public class NullDeleteUserQueue : IDeleteUserQueue
{
    public Task SendMessage(User user) => Task.CompletedTask;
}
