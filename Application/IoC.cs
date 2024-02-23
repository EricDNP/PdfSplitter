using Application.DocumentFiles.Handlers;
using Application.DocumentFiles.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class IoC
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IDocumentFileHandler, DocumentFileHandler>();

            return services;
        }
    }
}
