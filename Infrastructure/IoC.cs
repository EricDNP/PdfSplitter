using Application.Common.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class IoC
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddTransient<IBlobStorageService, BlobStorageService>();
            serviceCollection.AddTransient<IPdfManagerService, PdfManagerService>();

            return serviceCollection;
        }
    }
}
