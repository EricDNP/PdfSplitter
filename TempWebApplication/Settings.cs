using Application;
using Infrastructure;

namespace TempWebApplication
{
    public class AppSetup
    {
        public ConfigurationManager Configuration { get; set; }

        public AppSetup(ConfigurationManager configuration)
        {
            Configuration = configuration;
        }

        public void RegisterServices (IServiceCollection services, IWebHostEnvironment env)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);
        }
    }
}
