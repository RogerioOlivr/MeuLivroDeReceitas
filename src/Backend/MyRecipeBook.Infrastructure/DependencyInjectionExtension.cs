using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories.Token;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.OpenAI;
using MyRecipeBook.Domain.Services.ServiceBus;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Cryptography;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using MyRecipeBook.Infrastructure.Security.Tokens.Refresh;
using MyRecipeBook.Infrastructure.Services.LoggedUser;
using MyRecipeBook.Infrastructure.Services.OpenAI;
using MyRecipeBook.Infrastructure.Services.ServiceBus;
using MyRecipeBook.Infrastructure.Services.Storage;
using MyRecipeBook.Infrastructure.Migrations.Versions;
using FluentMigrator.Runner;
using OpenAI;
using OpenAI.Chat;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddFluentMigrator(services, configuration);
        AddRepositories(services);
        AddServices(services, configuration);
        AddBlobStorage(services, configuration);
        AddServiceBus(services, configuration);
        AddOpenAI(services, configuration);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.IsUnitTestEnviroment())
            return;

        var databaseType = configuration.DatabaseType();
        var connectionString = configuration.ConnetionString();

        if (databaseType == DatabaseType.MySql)
        {
            services.AddDbContext<MyRecipeBookDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
        }
        else
        {
            services.AddDbContext<MyRecipeBookDbContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.IsUnitTestEnviroment())
            return;

        var databaseType = configuration.DatabaseType();
        var connectionString = configuration.ConnetionString();

        services.AddFluentMigratorCore()
            .ConfigureRunner(rb =>
            {
                if (databaseType == DatabaseType.MySql)
                {
                    rb.AddMySql5()
                      .WithGlobalConnectionString(connectionString)
                      .ScanIn(typeof(VersionBase).Assembly).For.Migrations();
                }
                else
                {
                    rb.AddSqlServer()
                      .WithGlobalConnectionString(connectionString)
                      .ScanIn(typeof(VersionBase).Assembly).For.Migrations();
                }
            })
            .AddLogging(lb => lb.AddFluentMigratorConsole());
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
        services.AddScoped<IUserDeleteOnlyRepository, UserRepository>();
        services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
        services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ILoggedUser, LoggedUser>();
        services.AddScoped<IPasswordEncripter, BCryptNet>();
        
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey")!;
        services.AddScoped<IAccessTokenGenerator>(_ => new JwtTokenGenerator(expirationTimeMinutes, signingKey));
        services.AddScoped<IAccessTokenValidator>(_ => new JwtTokenValidator(signingKey));
        
        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
    }

    private static void AddBlobStorage(IServiceCollection services, IConfiguration configuration)
    {
        var azureStorageConnectionString = configuration.GetValue<string>("Settings:BlobStorage:Azure");

        if (!string.IsNullOrWhiteSpace(azureStorageConnectionString))
        {
            services.AddSingleton(new BlobServiceClient(azureStorageConnectionString));
            services.AddScoped<IBlobStorageService, AzureStorageService>();
        }
        else
        {
            services.AddScoped<IBlobStorageService, NullBlobStorageService>();
        }
    }

    private static void AddServiceBus(IServiceCollection services, IConfiguration configuration)
    {
        var serviceBusConnectionString = configuration.GetValue<string>("Settings:ServiceBus:DeleteUserAccount");
        var queueName = configuration.GetValue<string>("Settings:ServiceBus:DeleteUserQueueName") ?? "delete-user-account";

        if (!string.IsNullOrWhiteSpace(serviceBusConnectionString))
        {
            var serviceBusClient = new ServiceBusClient(serviceBusConnectionString);
            
            services.AddSingleton(serviceBusClient);
            services.AddSingleton(sp => serviceBusClient.CreateSender(queueName));
            services.AddSingleton(sp => serviceBusClient.CreateProcessor(queueName));
            services.AddScoped<IDeleteUserQueue, DeleteUserQueue>();
            services.AddSingleton<DeleteUserProcessor>(sp =>
            {
                var processor = sp.GetRequiredService<ServiceBusProcessor>();
                return new DeleteUserProcessor(processor);
            });
        }
        else
        {
            services.AddScoped<IDeleteUserQueue, NullDeleteUserQueue>();
        }
    }

    private static void AddOpenAI(IServiceCollection services, IConfiguration configuration)
    {
        var openAiApiKey = configuration.GetValue<string>("Settings:OpenAI:ApiKey");

        if (!string.IsNullOrWhiteSpace(openAiApiKey))
        {
            services.AddSingleton(new OpenAIClient(openAiApiKey));
            services.AddScoped<ChatClient>(sp =>
            {
                var openAiClient = sp.GetRequiredService<OpenAIClient>();
                return openAiClient.GetChatClient("gpt-4");
            });
            services.AddScoped<IGenerateRecipeAI, ChatGptService>();
        }
        else
        {
            services.AddScoped<IGenerateRecipeAI, NullGenerateRecipeAI>();
        }
    }
}
