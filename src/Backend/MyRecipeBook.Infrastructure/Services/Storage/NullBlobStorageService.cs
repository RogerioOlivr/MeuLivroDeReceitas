using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Domain.Entities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyRecipeBook.Infrastructure.Services.Storage;

public class NullBlobStorageService : IBlobStorageService
{
    public Task Delete(User user, string fileName) => Task.CompletedTask;

    public Task DeleteContainer(Guid userIdentifier) => Task.CompletedTask;

    public Task<string> GetFileUrl(User user, string fileName) => Task.FromResult(string.Empty);

    public Task Upload(User user, Stream file, string fileName) => Task.CompletedTask;
}
